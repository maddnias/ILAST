using System;

namespace ILAST.AST.Base
{
    public abstract class Variable
    {
        public Type Type { get;set;}
        public int Index { get; set; }
        public object Tag { get; set; }

        protected Variable(int index)
        {
            Index = index;
        }

        public abstract override string ToString();
    }
}