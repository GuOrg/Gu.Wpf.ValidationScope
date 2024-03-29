﻿#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
namespace Gu.Wpf.ValidationScope;

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

/// <summary>
/// A <see cref="TypeConverter"/> for <see cref="InputTypeCollection"/>.
/// </summary>
public class InputTypeCollectionConverter : TypeConverter
{
    private static readonly char[] SeparatorChars = { ',', ' ' };

    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) ||
               sourceType == typeof(Type) ||
               sourceType == typeof(TypeExtension) ||
               typeof(IEnumerable<Type>).IsAssignableFrom(sourceType);
    }

    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return false;
    }

    /// <inheritdoc />
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return value switch
        {
            string text => ConvertFromText(text),
            IEnumerable<Type> types => new InputTypeCollection(types),
            Type type => new InputTypeCollection { type },
            TypeExtension typeExtension => new InputTypeCollection { typeExtension.Type },
            _ => base.ConvertFrom(context, culture, value),
        };
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <inheritdoc />
    [SecurityCritical]
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
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
        private static readonly object Gate = new();
        private static readonly HashSet<Type> Types = new();
        private static readonly List<Type> KnownInputTypes = InputTypeCollection.Default.ToList();
        private static readonly HashSet<string> ExcludedAssemblies = new()
        {
            "Castle.Core",
            "JetBrains.ReSharper.TaskRunnerFramework",
            "JetBrains.ReSharper.UnitTestRunner.nUnit",
            "JetBrains.ReSharper.UnitTestRunner.nUnit30",
            "Mono.Cecil",
            "Microsoft.Windows.Design.Extensibility",
            "Microsoft.Windows.Design.Interaction",
            "Microsoft.VisualStudio.DesignTools.Designer",
            "Microsoft.VisualStudio.DesignTools.Designer.resources",
            "Microsoft.VisualStudio.DesignTools.DesignerContract",
            "Microsoft.VisualStudio.DesignTools.DesignerHost",
            "Microsoft.VisualStudio.DesignTools.DesignerHost.resources",
            "Microsoft.VisualStudio.DesignTools.Markup",
            "Microsoft.VisualStudio.DesignTools.Platform",
            "Microsoft.VisualStudio.DesignTools.Platform.resources",
            "Microsoft.VisualStudio.DesignTools.Utility",
            "Microsoft.VisualStudio.DesignTools.Utility.resources",
            "Microsoft.VisualStudio.DesignTools.WpfDesigner",
            "Microsoft.VisualStudio.DesignTools.XamlDesigner",
            "Microsoft.VisualStudio.DesignTools.XamlDesigner.resources",
            "Microsoft.VisualStudio.RemoteControl",
            "Microsoft.VisualStudio.Telemetry",
            "Microsoft.VisualStudio.Utilities.Internal",
            "mscorlib",
            "Newtonsoft.Json",
            "nunit.engine",
            "nunit.engine.api",
            "nunit.framework",
            "PresentationFramework.Aero",
            "SMDiagnostics",
            "System",
            "System.ComponentModel.DataAnnotations",
            "System.Configuration",
            "System.Core",
            "System.Data",
            "System.Drawing",
            "System.Numerics",
            "System.Runtime.Remoting",
            "System.Runtime.Serialization",
            "System.ServiceModel.Internals",
            "System.Web",
            "System.Xml",
            "System.Xml.Linq",
            "System.Security",
            "System.Windows.Forms",
            "TestStack.White",
            "UIAutomationClient",
            "UIAutomationProvider",
            "UIAutomationTypes",
            "XDesProc",
        };

        private static readonly StringBuilder ErrorBuilder = new();

        static CompatibleTypeCache()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AddCompatibleTypes(assembly);
            }

            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) => AddCompatibleTypes(args.LoadedAssembly);
        }

        // ReSharper disable once UnusedMember.Local
        internal static string ErrorText => ErrorBuilder.ToString();

        internal static Type FindType(string typeName)
        {
            Type? match;
            try
            {
                lock (Gate)
                {
                    match = KnownInputTypes.Find(t => IsMatch(t, typeName));
                    if (match != null)
                    {
                        return match;
                    }

                    match = Types.SingleOrDefault(t => IsMatch(t, typeName));
                }
            }
            catch (Exception)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression
                var errorBuilder = new StringBuilder();
#pragma warning disable CA1305 // Specify IFormatProvider
                errorBuilder.AppendLine($"Found more than one match for {typeName}");
#pragma warning restore CA1305 // Specify IFormatProvider
                lock (Gate)
                {
                    var matches = Types.Where(t => IsMatch(t, typeName));
                    foreach (var type in matches)
                    {
#pragma warning disable CA1305 // Specify IFormatProvider
                        errorBuilder.AppendLine($"  - {type.FullName} in assembly: {type.Assembly.FullName}");
#pragma warning restore CA1305 // Specify IFormatProvider
                    }
                }

                if (!IsFullName(typeName))
                {
#pragma warning disable CA1305 // Specify IFormatProvider
                    errorBuilder.AppendLine($"Try specifying full name: {typeof(TextBox).FullName}");
#pragma warning restore CA1305 // Specify IFormatProvider
                }

                throw new InvalidOperationException(errorBuilder.ToString());
            }

            if (match is null)
            {
                throw new InvalidOperationException($"Did not find a match for for {typeName}");
            }
#pragma warning restore IDE0079 // Remove unnecessary suppression
            return match;
        }

        private static bool IsMatch(Type type, string name)
        {
            return type.Name == name || type.FullName == name;
        }

        private static bool IsFullName(string typeName)
        {
            return typeName.Contains('.');
        }

        private static void AddCompatibleTypes(Assembly assembly)
        {
            Debug.WriteLine(assembly.FullName);

            var name = assembly.GetName().Name;
            if (name is null ||
                ExcludedAssemblies.Contains(name))
            {
                return;
            }

            try
            {
                lock (Gate)
                {
                    foreach (var compatibleType in assembly.GetTypes().Where(InputTypeCollection.IsCompatibleType))
                    {
                        if (Types.Add(compatibleType))
                        {
                            if (KnownInputTypes.Contains(compatibleType))
                            {
                                continue;
                            }

                            if (KnownInputTypes.Any(t => t.IsAssignableFrom(compatibleType)))
                            {
                                KnownInputTypes.Add(compatibleType);
                            }
                        }
                        else
                        {
                            Trace.WriteLine($"Type {compatibleType.FullName} was already added");
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (ex.LoaderExceptions is { } loaderExceptions)
                {
                    // http://stackoverflow.com/a/8824250/1069200
                    foreach (var loaderException in loaderExceptions)
                    {
                        ErrorBuilder.AppendLine(loaderException?.Message ?? "null");
                        if (loaderException is FileNotFoundException fileNotFoundException)
                        {
                            ErrorBuilder.AppendLine(fileNotFoundException.FusionLog);
                        }

                        ErrorBuilder.AppendLine();
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                var message = $"Could not process assembly {assembly.FullName}. Exception: {e.Message}";
                Trace.WriteLine(message);
                ErrorBuilder.AppendLine(message);
            }
        }
    }
}
