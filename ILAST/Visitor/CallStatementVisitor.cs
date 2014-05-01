using System;
using ILAST.AST;
using ILAST.Visitor.Base;

namespace ILAST.Visitor
{
    class CallStatementVisitor : ElementVisitor
    {
        public override void Visit(LiteralExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(LiteralLongExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(BinOpExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(StringExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ReturnExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(UnaryOpExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(VariableExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ConditionalBranchExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(AssignmentStatement statement)
        {
            throw new NotImplementedException();
        }

        public override void Visit(UnconditionalBranchExpression statement)
        {
            throw new NotImplementedException();
        }

        public override void Visit(CallStatement statement)
        {
            var tcv = new TypeCheckVisitor();

            if (statement.ArgumentExpressions.Count != statement.Target.Parameters.Count)
                throw new Exception("Parameter count mismatch");

            for (var i = 0; i < statement.ArgumentExpressions.Count; i++)
            {
                statement.ArgumentExpressions[i].AcceptVisitor(tcv);
                if (tcv.ResultType != statement.Target.Parameters[i].Type.ToReflectionType())
                    throw new Exception("Parameter type mismatch");
            }
        }

        public override void Visit(ForLoopStatement statement)
        {
            throw new NotImplementedException();
        }
    }
}
