using System.Collections.Generic;
using System.Linq;
using de4dot.blocks;
using dnlib.DotNet;
using ILAST.AST;

namespace ILAST
{
    internal class BodyAnalyzer
    {
        public List<Expression> Expressions { get; set; }
        private MethodDef Method { get; set; }

        public BodyAnalyzer(MethodDef method)
        {
            Expressions = new List<Expression>();
            Method = method;
        }

        public void ParseBody()
        {
            var blocks = new Blocks(Method).MethodBlocks.GetAllBlocks();

            foreach (var blockAnalyzer in blocks.Select(block => new BlockAnalyzer(block)))
                Expressions.AddRange(blockAnalyzer.GenerateExpressions());
        }


    }
}
