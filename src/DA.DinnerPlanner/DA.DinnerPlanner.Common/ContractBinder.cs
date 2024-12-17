using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DA.DinnerPlanner.Common
{
	/// <ChangeLog>
		/// <Create Datum="17.12.2024" Entwickler="DA" />
		/// </ChangeLog>
	public static class ContractBinder
	{
		private static HashSet<Tuple<Type, Type>> container;
		private static int counter = 0;

		static ContractBinder()
		{
			container = new HashSet<Tuple<Type, Type>>();
		}
		private static void Bind(Type InterfaceType, Type ImplementationType)
		{
			Tuple<Type, Type> item = new Tuple<Type, Type>(InterfaceType, ImplementationType);
			container.Add(item);
			counter++;
		}

		public static void CreateBindings()
		{
			//Bind(typeof(Contracts.Encryption.IEncHasher), typeof(Encryption.MD5Hasher));
			Bind(typeof(Model.Contracts.IDinnerPlannerContext), typeof(Model.DinnerPlannerContext));
		}
		public static TInterface GetObject<TInterface>()
		{
			if (0 == counter)
				CreateBindings();
			foreach (Tuple<Type, Type> bindings in container)
			{
				if (bindings.Item1.Equals(typeof(TInterface)))
				{
					return (TInterface)Activator.CreateInstance(bindings.Item2);
				}
			}
			return default;
		}
	}
}
