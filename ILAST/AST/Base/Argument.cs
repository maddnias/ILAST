namespace ILAST.AST.Base
{
    class Argument : Variable
    {
        public Argument(int index) 
            : base(index)
        {
        }

        public override string ToString()
        {
            return "arg_" + Index;
        }
    }
}
