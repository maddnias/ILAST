using System;
using System.Collections.Generic;
using de4dot.blocks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ILAST.AST;
using ILAST.AST.Base;

namespace ILAST
{
    public static class Extensions
    {
        public static MethodDef ResolveCallTarget(this Instr instr)
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

        public static bool IsCall(this Instr instr)
        {
            return instr.OpCode.Code == Code.Call || instr.OpCode.Code == Code.Calli ||
                   instr.OpCode.Code == Code.Callvirt;
        }

        public static Expression GetPrevious(this Element element, int count)
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

            return cur as Expression;
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
