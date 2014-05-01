using System.Collections.Generic;
using System.Linq;
using de4dot.blocks;
using dnlib.DotNet;
using ILAST.AST.Base;
using ILASTLocal = ILAST.AST.Base.Local;

namespace ILAST
{
    sealed class BlockAnalyzer
    {
        public MethodDef Method { get; set; }
        public Block Block { get; set; }
        public List<Element> Elements { get; set; }

        public BlockAnalyzer(Block block, MethodDef method)
        {
            Block = block;
            Method = method;
            Elements = GenerateExpressions().ToList();
            LinkElements();
        }

        IEnumerable<Element> GenerateExpressions()
        {
            var exprFactory = new ExpressionFactory();
            return Block.Instructions.SelectMany(instr => exprFactory.GetElements(instr, Method));
        }

        void LinkElements()
        {
            for (var i = 0; i < Elements.Count; i++)
            {
                if (i != 0)
                    Elements[i].Previous = Elements[i - 1];
                if (i != Elements.Count -1)
                    Elements[i].Next = Elements[i + 1];
            }
        }
    }
}
