using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ILAST.AST;
using ILAST.AST.Base;

namespace ILAST
{
    public static class Extensions
    {
        public static bool IsForLoop(this ConditionalBranchExpression expr, MethodDef method)
        {
            if (!(expr.TargetElement.Previous is UnconditionalBranchExpression)) return false;
            return (expr.TargetElement as VariableExpression) != null;
        }

        public static bool IsIncrement(this Expression expr)
        {
            if (!(expr is BinOpExpression))
                return false;

            if ((expr as BinOpExpression).Left is VariableExpression &&
                (expr as BinOpExpression).Right is LiteralExpression)
                if (((expr as BinOpExpression).Right as LiteralExpression).Value == 1)
                    return true;
            if ((expr as BinOpExpression).Left is LiteralExpression &&
                (expr as BinOpExpression).Right is VariableExpression)
                if (((expr as BinOpExpression).Left as LiteralExpression).Value == 1)
                    return true;

            return false;
        }

        public static Element ResolveTargetElement(this Expression expr)
        {
            if (expr is UnconditionalBranchExpression ||
                expr is ConditionalBranchExpression)
            {
                var targetInstr = expr.AssociatedInstruction.Operand as Instruction;

                //search forward
                Element cur = expr;
                while (cur.Next != null)
                {
                    cur = cur.Next;
                    if (cur.AssociatedInstruction == targetInstr)
                        return cur;
                }
                //search backwards
                while (cur.Previous != null)
                {
                    cur = cur.Previous;
                    if (cur.AssociatedInstruction == targetInstr)
                        return cur;
                }
            }

            return null;
        }

        public static MethodDef ResolveCallTarget(this Instruction instr)
        {
            if (!instr.IsCall())
                return null;

            object operand = instr.Operand;

            if (operand is MemberRef)
                return (operand as MemberRef).ResolveMethodThrow();
            if (operand is MethodDef)
                return operand as MethodDef;

            return null;
        }

        public static bool IsCall(this Instruction instr)
        {
            return instr.OpCode.Code == Code.Call || instr.OpCode.Code == Code.Calli ||
                   instr.OpCode.Code == Code.Callvirt;
        }

        public static Element GetPrevious(this Element element, int count)
        {
            var realCount = count;
            var cur = element;

            for (var i = 0; i < count -1; i++)
            {
                cur = cur.Previous;
                realCount += cur.ElementSize - 1;
            }

            cur = element;
            for (var i = 0; i < realCount; i++)
            {
                cur = cur.Previous;
            }

            return cur;
        }

        public static Expression SafeGetExpression(this List<Element> elements, int start, int count)
        {
            var extraCount = 1;

            for (var i = start -1; i > start - count; i--)
            {
                if (elements[i] is Expression)
                {
                    if (elements[i] is BinOpExpression)
                        extraCount += 2;
                }
                if (elements[i] is Statement)
                {
                    if (elements[i] is AssignmentStatement)
                        extraCount += 2;
                }
                else
                    extraCount++;
            }

            return elements[start -  extraCount] as Expression;
        }

        public static Type ToReflectionType(this TypeSig type)
        {
            return Type.GetType(type.ReflectionFullName);
        }
    }
}
