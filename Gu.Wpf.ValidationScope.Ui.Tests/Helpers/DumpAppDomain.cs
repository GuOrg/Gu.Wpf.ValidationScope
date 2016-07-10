namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using NUnit.Framework;

    [Explicit("Script")]
    public class DumpAppDomain
    {
        [Test]
        public void DumpNotExcludedAssemblies()
        {
            var excludedAssemblies = ExcludedAssemblies();
            var notExcluded = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(a => !excludedAssemblies.Contains(a.GetName().Name))
                                      .Select(a => a.GetName().Name)
                                      .OrderBy(x => x)
                                      .ToList();
            foreach (var assemblyName in notExcluded)
            {
                Console.WriteLine($"\"{assemblyName}\",");
            }
        }

        [Test]
        public void DumpExcludedAssembliesSorted()
        {
            var excludedAssemblies = ExcludedAssemblies()
                .OrderBy(x => x)
                .ToArray();
            foreach (var name in excludedAssemblies)
            {
                Console.WriteLine($"\"{name}\",");
            }
        }

        private static HashSet<string> ExcludedAssemblies()
        {
            // ReSharper disable once PossibleNullReferenceException
            var hashSet = (HashSet<string>)typeof(InputTypeCollectionConverter).GetNestedType("CompatibleTypeCache", BindingFlags.Static | BindingFlags.NonPublic)
                                                                               .GetField("ExcludedAssemblies", BindingFlags.Static | BindingFlags.NonPublic)
                                                                               .GetValue(null);
            return new HashSet<string>(hashSet);
        }
    }
}
