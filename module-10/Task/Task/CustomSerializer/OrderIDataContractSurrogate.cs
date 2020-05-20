using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.CustomSerializer
{
	public class OrderIDataContractSurrogate : IDataContractSurrogate
	{
		public Type GetDataContractType(Type type)
		{
			return type;
		}

		public object GetObjectToSerialize(object obj, Type targetType)
		{
			if (!targetType.IsInterface)
			{
				var result = UnProxy(obj, targetType);
				return result;
			}

			return obj;
		}

		private object UnProxy (object obj, Type targetType) 
		{
			var result = Activator.CreateInstance(targetType);
			foreach (var prop in targetType.GetProperties())
			{
				var v = prop.GetValue(obj);
				prop.SetValue(result, v);
			}
			return result;
		}

		public object GetDeserializedObject(object obj, Type targetType)
		{
			return obj;
		}


		public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
		{
			throw new NotImplementedException();
		}

		public object GetCustomDataToExport(Type clrType, Type dataContractType)
		{
			throw new NotImplementedException();
		}

		public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
		{
			throw new NotImplementedException();
		}


		public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
		{
			throw new NotImplementedException();
		}

		public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
		{
			throw new NotImplementedException();
		}
	}
}
