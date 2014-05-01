using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class VariableExpression : Expression
    {
        internal MethodDef Method { get; set; }
        private Variable _variable;

        public VariableExpression(Instruction instr, MethodDef method)
            : base(instr)
        {
            Method = method;
        }

        public override void Populate()
        {

        }

        public Variable Variable
        {
            get
            {
                return _variable;
            }
            set
            {
                _variable = value;
                _variable.Type = Method.Body.Variables[_variable.Index].Type.ToReflectionType();
            } 
        }

        public override int ElementSize { get { return 1; } }
        public override bool CanSimplify { get { return false; } }

        public override IEnumerable<Element> RemovableElements
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Variable.ToString();
        }
    }
}