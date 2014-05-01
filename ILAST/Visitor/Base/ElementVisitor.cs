using ILAST.AST;

namespace ILAST.Visitor.Base
{
    public abstract class ElementVisitor
    {
        public abstract void Visit(LiteralExpression expression);
        public abstract void Visit(LiteralLongExpression expression);
        public abstract void Visit(BinOpExpression expression);
        public abstract void Visit(StringExpression expression);
        public abstract void Visit(ReturnExpression expression);
        public abstract void Visit(UnaryOpExpression expression);
        public abstract void Visit(VariableExpression expression);
        public abstract void Visit(ConditionalExpression expression);
        public abstract void Visit(AssignmentStatement statement);
        public abstract void Visit(UnconditionalBranchStatement statement);
        public abstract void Visit(CallStatement statement);
    }
}
