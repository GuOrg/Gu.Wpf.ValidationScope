namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Runtime.CompilerServices;

    public static class AutomationIDs
    {
        public static readonly string MainWindow = Create();
        public static readonly string OneLevelScopeTab = Create();
        public static readonly string TwoLevelScopeTab = Create();
        public static readonly string ScopeWithDataTemplatesTab = Create();
        public static readonly string ScopeWithControlTemplatesTab = Create();
        public static readonly string ComplicatedScopeTab = Create();
        public static readonly string DynamicScopeTab = Create();
        public static readonly string DataGridScopeTab = Create();

        public static readonly string TextBox1 = Create();
        public static readonly string TextBox2 = Create();
        public static readonly string ErrorList = Create();
        public static readonly string ErrorText = Create();
        
        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
