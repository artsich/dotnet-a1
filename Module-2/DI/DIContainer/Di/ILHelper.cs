using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Di
{
    //Class not used, just save this code here.
    public class ILHelper
    {
        public static Func<object[], object> CreateObject(ConstructorInfo ctor)
        {
            Func<object[], object> factoryMethod = null;

            var parameters = ctor.GetParameters();

            var dm = new DynamicMethod(string.Format("_CreationFactory_{0}", Guid.NewGuid()), typeof(object), new Type[] { typeof(object[]) }, true);
            var il = dm.GetILGenerator();

            il.DeclareLocal(typeof(int));
            il.DeclareLocal(typeof(object));

            il.BeginExceptionBlock();

            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_0);

            for (int i = 0; i < parameters.Length; ++i)
            {
                EmitInt32(il, i);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldarg_0);
                EmitInt32(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                var paramType = parameters[i].ParameterType;
                if (paramType != typeof(object))
                {
                    il.Emit(OpCodes.Unbox_Any, paramType);
                }
            }

            il.Emit(OpCodes.Newobj, ctor); //[new-object]
            il.Emit(OpCodes.Stloc_1); // nothing

            il.BeginCatchBlock(typeof(Exception)); // stack is Exception
            il.EndExceptionBlock();

            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);

            factoryMethod = (Func<object[], object>)dm.CreateDelegate(typeof(Func<object[], object>));

            return factoryMethod;
        }

        public static void EmitInt32(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (value >= -128 && value <= 127)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }
    }
}
