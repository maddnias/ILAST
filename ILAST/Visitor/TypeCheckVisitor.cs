using System;
using ILAST.AST;
using ILAST.Visitor.Base;

namespace ILAST.Visitor
{
    class TypeCheckVisitor : ElementVisitor
    {
        public Type ResultType { get; set; }

        public override void Visit(LiteralExpression expression)
        {
            ResultType = typeof (int);
        }

        public override void Visit(LiteralLongExpression expression)
        {
            ResultType = typeof (long);
        }

        public override void Visit(BinOpExpression expression)
        {
            expression.Left.AcceptVisitor(this);
            var type1 = ResultType;
            expression.Right.AcceptVisitor(this);
            var type2 = ResultType;

            if (expression.Operation == BinOps.Add ||
                expression.Operation == BinOps.Div ||
                expression.Operation == BinOps.Mul ||
                expression.Operation == BinOps.Sub)
            {
                if (type1 == typeof (int))
                {
                    if(type2 == typeof(long))
                        throw new InvalidProgramException("Operation types are invalid");

                    ResultType = typeof (int);
                }
                else if (type1 == typeof (long))
                {
                    if (type2 == typeof (int))
                        throw new InvalidProgramException("Operation types are invalid");

                    ResultType = typeof (long);
                }
            }
        }

        public override void Visit(StringExpression expression)
        {
            ResultType = typeof (string);
        }

        public override void Visit(ReturnExpression expression)
        {
            expression.ReturnValue.AcceptVisitor(this);
        }

        public override void Visit(UnaryOpExpression expression)
        {
            expression.Value.AcceptVisitor(this);
        }

        public override void Visit(VariableExpression expression)
        {
            ResultType = expression.Variable.Type;
        }

        public override void Visit(ConditionalExpression expression)
        {
            throw new NotImplementedException();
        }

        public override void Visit(AssignmentStatement statement)
        {
            statement.Value.AcceptVisitor(this);
            statement.Target.AcceptVisitor(this);
        }

        public override void Visit(UnconditionalBranchStatement statement)
        {

        }

        public override void Visit(CallStatement statement)
        {
            ResultType = statement.Target.ReturnType.ToReflectionType();
        }
    }
}
