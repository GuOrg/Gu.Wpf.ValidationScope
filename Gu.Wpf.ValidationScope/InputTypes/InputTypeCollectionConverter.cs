namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Windows;
    using System.Windows.Markup;

    public class InputTypeCollectionConverter : TypeConverter
    {
        private static readonly char[] SeparatorChars = { ',', ' ' };

        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            return sourceType == typeof(string) ||
                   sourceType == typeof(Type) ||
                   sourceType == typeof(TypeExtension) ||
                   typeof(IEnumerable<Type>).IsAssignableFrom(sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
        {
            var text = source as string;
            if (text != null)
            {
                return ConvertFromText(text);
            }

            var types = source as IEnumerable<Type>;
            if (types != null)
            {
                return new InputTypeCollection(types);
            }

            var type = source as Type;
            if (type != null)
            {
                return new InputTypeCollection { type };
            }

            var typeExtension = source as TypeExtension;
            if (typeExtension != null)
            {
                return new InputTypeCollection { typeExtension.Type };
            }

            return base.ConvertFrom(typeDescriptorContext, cultureInfo, source);
        }

        [SecurityCritical]
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            throw new NotSupportedException();
        }

        private static InputTypeCollection ConvertFromText(string text)
        {
            var typeNames = text.Split(SeparatorChars, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();
            var inputTypeCollection = new InputTypeCollection();
            foreach (var typeName in typeNames)
            {
                var match = CompatibleTypeCache.FindType(typeName);
                inputTypeCollection.Add(match);
            }

            return inputTypeCollection;
        }

        private static class CompatibleTypeCache
        {
            private static readonly List<Type> Types = new List<Type>();

            static CompatibleTypeCache()
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    GetCompatibleTypes(assembly);
                }

                AppDomain.CurrentDomain.AssemblyLoad += (sender, args) => GetCompatibleTypes(args.LoadedAssembly);
            }

            public static Type FindType(string typeName)
            {
                Type match;
                try
                {
                    match = Types.SingleOrDefault(x => x.Name == typeName);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException($"Found more than one match for {typeName}");
                }

                if (match == null)
                {
                    throw new InvalidOperationException($"Did not find a match for for {typeName}");
                }

                return match;
            }

            private static void GetCompatibleTypes(Assembly assembly)
            {
                Debug.WriteLine(assembly.FullName);
                try
                {
                    Types.AddRange(assembly.ExportedTypes.Where(InputTypeCollection.IsCompatibleType));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Could not process assembly {assembly.FullName}. Exception: {e.Message}");
                    throw;
                }
            }
        }
    }
}