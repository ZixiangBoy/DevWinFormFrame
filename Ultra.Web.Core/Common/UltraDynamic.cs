using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Text;

namespace Ultra.Web.Core.Common
{
    public class UltraDynamic
    {
        private static UltraDynamic _def = new UltraDynamic();

        public virtual string Calc(string src)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.Add("System.dll");
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { this.GenCode(src) });
            if (results.Errors.HasErrors)
            {
                throw new Exception(results.Errors[0].ErrorText);
            }
            object obj2 = results.CompiledAssembly.CreateInstance("dynamicrun");
            return obj2.GetType().GetMethod("MathEval").Invoke(obj2, null).ToString();
        }

        protected virtual string GenCode(string exp)
        {
            return string.Format(this.GencodeFormat, exp);
        }

        public static UltraDynamic Default
        {
            get
            {
                return _def;
            }
        }

        protected virtual string GencodeFormat
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("using System;             \n");
                builder.Append("public class dynamicrun   \n");
                builder.Append("{{                         \n");
                builder.Append("public decimal MathEval() \n");
                builder.Append("{{                         \n");
                builder.Append(" return {0};              \n");
                builder.Append("}}                         \n");
                builder.Append("}}                         \n");
                return builder.ToString();
            }
        }
    }
}

