namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Text;
    using System.Windows.Controls;
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
            private static readonly HashSet<Type> Types = new HashSet<Type>();

            private static readonly HashSet<string> ExcludedAssemblies = new HashSet<string>
                                                                             {
                                                                                 "mscorlib",
                                                                                 "Microsoft.VisualStudio.DesignTools.DesignerContract",
                                                                                 "XDesProc",
                                                                                 "System",
                                                                                 "System.Core",
                                                                                 "Microsoft.VisualStudio.DesignTools.Utility",
                                                                                 "System.Runtime.Remoting",
                                                                                 "System.Configuration",
                                                                                 "System.Xml",
                                                                                 "System.Web",
                                                                                 "Microsoft.VisualStudio.DesignTools.Designer",
                                                                                 "Microsoft.VisualStudio.DesignTools.Platform",
                                                                                 "Microsoft.VisualStudio.DesignTools.Markup",
                                                                                 "Microsoft.VisualStudio.Telemetry",
                                                                                 "Microsoft.VisualStudio.Utilities.Internal",
                                                                                 "System.Runtime.Serialization",
                                                                                 "SMDiagnostics",
                                                                                 "System.ServiceModel.Internals",
                                                                                 "Microsoft.VisualStudio.RemoteControl",
                                                                                 "Newtonsoft.Json",
                                                                                 "Microsoft.Windows.Design.Extensibility",
                                                                                 "System.Numerics",
                                                                                 "System.ComponentModel.DataAnnotations",
                                                                                 "System.Xml.Linq",
                                                                                 "System.Data",
                                                                                 "Microsoft.VisualStudio.DesignTools.DesignerHost",
                                                                                 "Microsoft.VisualStudio.DesignTools.DesignerHost.resources",
                                                                                 "Microsoft.VisualStudio.DesignTools.Utility.resources",
                                                                                 "System.Drawing",
                                                                                 "PresentationFramework.Aero",
                                                                                 "Microsoft.VisualStudio.DesignTools.Designer.resources",
                                                                                 "Microsoft.Windows.Design.Interaction",
                                                                                 "Microsoft.VisualStudio.DesignTools.WpfDesigner",
                                                                                 "Microsoft.VisualStudio.DesignTools.XamlDesigner",
                                                                                 "Microsoft.VisualStudio.DesignTools.Platform.resources",
                                                                                 "Microsoft.VisualStudio.DesignTools.XamlDesigner.resources",
                                                                             };
            private static readonly StringBuilder ErrorBuilder = new StringBuilder();

            static CompatibleTypeCache()
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                // File.WriteAllLines(@"C:\Temp\Gu.Wpf.ValidationScope\Log.txt", assemblies.Select(x => $"\"{x.GetName().Name}\","));
                foreach (var assembly in assemblies)
                {
                    GetCompatibleTypes(assembly);
                }

                AppDomain.CurrentDomain.AssemblyLoad += (sender, args) => GetCompatibleTypes(args.LoadedAssembly);
            }

            public static string ErrorText => ErrorBuilder.ToString();

            public static Type FindType(string typeName)
            {
                Type match;
                try
                {
                    match = IsFullName(typeName)
                        ? Types.SingleOrDefault(x => x.FullName == typeName)
                        : Types.SingleOrDefault(x => x.Name == typeName);
                }
                catch (Exception)
                {
                    var errorBuilder = new StringBuilder();
                    errorBuilder.AppendLine($"Found more than one match for {typeName}");
                    var matches = IsFullName(typeName)
                        ? Types.Where(x => x.FullName == typeName)
                        : Types.Where(x => x.Name == typeName);
                    foreach (var type in matches)
                    {
                        errorBuilder.AppendLine($"  - {type.FullName}");
                    }

                    if (!IsFullName(typeName))
                    {
                        errorBuilder.AppendLine($"Try specifying full name: {typeof(TextBox).FullName}");
                    }

                    throw new InvalidOperationException(errorBuilder.ToString());
                }

                if (match == null)
                {
                    throw new InvalidOperationException($"Did not find a match for for {typeName}");
                }

                return match;
            }

            private static bool IsFullName(string typeName)
            {
                return typeName.Contains('.');
            }

            private static void GetCompatibleTypes(Assembly assembly)
            {
                Debug.WriteLine(assembly.FullName);
                if (ExcludedAssemblies.Contains(assembly.GetName().Name))
                {
                    return;
                }

                try
                {
                    Types.UnionWith(assembly.GetTypes().Where(InputTypeCollection.IsCompatibleType));
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // http://stackoverflow.com/a/8824250/1069200
                    foreach (Exception exSub in ex.LoaderExceptions)
                    {
                        ErrorBuilder.AppendLine(exSub.Message);
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound?.FusionLog))
                        {
                            ErrorBuilder.AppendLine(exFileNotFound.FusionLog);
                        }

                        ErrorBuilder.AppendLine();
                    }
                }
                catch (Exception e)
                {
                    ErrorBuilder.AppendLine($"Could not process assembly {assembly.FullName}. Exception: {e.Message}");
                }
            }
        }
    }
}
