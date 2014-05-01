namespace ILAST.AST.Base
{
    class Local : Variable
    {
        public Local(int index) 
            : base(index)
        {
        }

        public override string ToString()
        {
            return "var_" + Index;
        }
    }
}
