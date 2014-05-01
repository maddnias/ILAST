using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ILAST.AST;
using ILAST.AST.Base;
using Local = ILAST.AST.Base.Local;

namespace ILAST
{
    internal class ExpressionFactory : ElementFactory
    {
        public override IEnumerable<Element> GetElements(Instruction instr, MethodDef method)
        {
            switch (instr.OpCode.OperandType)
            {
                case OperandType.InlineI:
                case OperandType.ShortInlineI:
                case OperandType.ShortInlineR:
                    yield return new LiteralExpression(instr) { Value = Convert.ToInt32(instr.Operand) };
                    break;
                case OperandType.InlineI8:
                    yield return new LiteralLongExpression(instr) { Value = (long)instr.Operand };
                    break;
                case OperandType.InlineString:
                    yield return new StringExpression(instr) { Value = (string)instr.Operand };
                    break;
                case OperandType.InlineVar:
                    if (instr.IsLdloc())
                        yield return
                            new VariableExpression(instr, method)
                            {
                                Variable = new Local(((dnlib.DotNet.Emit.Local)instr.Operand).Index)
                            };
                    if (instr.IsLdarg())
                        yield return
                            new VariableExpression(instr, method) { Variable = new Argument(((Parameter)instr.Operand).Index) };
                    break;

            }

            if (instr.IsBr())
                yield return new UnconditionalBranchExpression(instr);

            if (instr.IsConditionalBranch())
                yield return HandleConditionalBranch(instr, method);

            if (instr.IsCall())
                yield return HandleCall(instr);

            if (instr.IsLdarg())
                yield return HandleLdarg(instr, method);

            if (instr.IsStloc())
            {
                yield return HandleStloc(instr, method);
                yield return new AssignmentStatement(instr);
            }

            if (instr.IsLdloc())
                yield return HandleLdloc(instr, method);

            if (instr.IsLdcI4() && instr.Operand == null)
                yield return new LiteralExpression(instr) { Value = instr.GetLdcI4Value() };

            switch (instr.OpCode.Code)
            {
                case Code.Ret:
                    yield return new ReturnExpression(instr);
                    break;

                case Code.Not:
                    yield return new UnaryOpExpression(instr) { Operation = UnaryOps.Not };
                    break;

                case Code.Neg:
                    yield return new UnaryOpExpression(instr) { Operation = UnaryOps.Negate };
                    break;
            }

            if (instr.OpCode.Code == Code.Sub)
                yield return new BinOpExpression(instr) { Operation = BinOps.Sub };
            if (instr.OpCode.Code == Code.Add)
                yield return new BinOpExpression(instr) { Operation = BinOps.Add };
            if (instr.OpCode.Code == Code.Mul)
                yield return new BinOpExpression(instr) { Operation = BinOps.Mul };
        }

        private static ConditionalBranchExpression HandleConditionalBranch(Instruction instr, MethodDef method)
        {
            var expr = new ConditionalBranchExpression(instr);

            switch (instr.OpCode.Code)
            {
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                    expr.Operation = ConditionalOps.GreaterThanOrEqual;
                    break;
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                    expr.Operation = ConditionalOps.GreaterThan;
                    break;

                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                    expr.Operation = ConditionalOps.LessThanOrEqual;
                    break;

                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                    expr.Operation = ConditionalOps.LessThan;
                    break;
            }

            return expr;
        }

        static CallStatement HandleCall(Instruction instr)
        {
            return new CallStatement(instr, instr.ResolveCallTarget());
        }

        static VariableExpression HandleLdarg(Instruction instr, MethodDef method)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Ldarg_0:
                    return new VariableExpression(instr, method) { Variable = new Argument(0) };
                case Code.Ldarg_1:
                    return new VariableExpression(instr, method) { Variable = new Argument(1) };
                case Code.Ldarg_2:
                    return new VariableExpression(instr, method) { Variable = new Argument(2) };
                case Code.Ldarg_3:
                    return new VariableExpression(instr, method) { Variable = new Argument(3) };
                default:
                    throw new Exception("Should not happen");
            }
        }

        static VariableExpression HandleStloc(Instruction instr, MethodDef method)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Stloc_0:
                    return new VariableExpression(instr, method) { Variable = new Local(0) };
                case Code.Stloc_1:
                    return new VariableExpression(instr, method) { Variable = new Local(1) };
                case Code.Stloc_2:
                    return new VariableExpression(instr, method) { Variable = new Local(2) };
                case Code.Stloc_3:
                    return new VariableExpression(instr, method) { Variable = new Local(3) };
                default:
                    throw new Exception("Should not happen");
            }
        }

        static VariableExpression HandleLdloc(Instruction instr, MethodDef method)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Ldloc_0:
                    return new VariableExpression(instr, method) { Variable = new Local(0) };
                case Code.Ldloc_1:
                    return new VariableExpression(instr, method) { Variable = new Local(1) };
                case Code.Ldloc_2:
                    return new VariableExpression(instr, method) { Variable = new Local(2) };
                case Code.Ldloc_3:
                    return new VariableExpression(instr, method) { Variable = new Local(3) };
                default:
                    throw new Exception("Should not happen");
            }
        }
    }
}