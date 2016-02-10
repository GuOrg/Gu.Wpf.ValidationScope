namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Linq;
    using System.Security;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class InputTypeCollectionConverter : TypeConverter
    {
        private static readonly TypeExtensionTypeConverter TypeExtensionConverter = new TypeExtensionTypeConverter();
        private static readonly char[] SeparatorChars = { ',', ' ' };

        private static readonly IReadOnlyList<Type> CompatibleTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                                               .SelectMany(a => a.GetExportedTypes())
                                                                               .Where(InputTypeCollection.IsCompatibleType)
                                                                               .ToArray();


        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            return sourceType == typeof(string);
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
                var typeNames = text.Split(SeparatorChars, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(x => x.Trim())
                                  .ToArray();
                var inputTypeCollection = new InputTypeCollection();
                foreach (var typeName in typeNames)
                {
                    Type match;
                    try
                    {
                        match = CompatibleTypes.SingleOrDefault(x => x.Name == typeName);
                    }
                    catch (Exception)
                    {
                        throw new InvalidOperationException($"Found more than one match for {typeName}");
                    }

                    if (match == null)
                    {
                        throw new InvalidOperationException($"Did not find a match for for {typeName}");
                    }

                    inputTypeCollection.Add(match);
                }

                return inputTypeCollection;
            }

            return base.ConvertFrom(typeDescriptorContext, cultureInfo, source);
        }

        [SecurityCritical]
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            throw new NotSupportedException();
        }

        private class TypeExtensionTypeConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    return true;
                }

                return base.CanConvertTo(context, destinationType);
            }

            [SecurityCritical]
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (!(destinationType == typeof(InstanceDescriptor)))
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }

                var typeExtension = value as TypeExtension;
                if (typeExtension == null)
                {
                    throw new ArgumentException($"{value} must be of type {typeof(TypeExtension).Name}");
                }

                var constructorInfo = typeof(TypeExtension).GetConstructor(new Type[1] { typeof(Type) });
                var args = new object[1] { (object)typeExtension.Type };
                return new InstanceDescriptor(constructorInfo, args);
            }
        }
    }
}