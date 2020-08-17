using System.Threading.Tasks;
using FluentAssertions;

namespace Functional.Fluent
{
	public static class FluentOptionExtensions
	{
		public static OptionsAssertions<TSome> Should<TSome>(this Option<TSome> instance)
			=> new OptionsAssertions<TSome>(instance);

		public static async Task<OptionsAssertions<TSome>> Should<TSome>(this Task<Option<TSome>> instance)
			=> new OptionsAssertions<TSome>(await instance);

		public static async Task<AndConstraint<OptionsAssertions<TSome>>> BeSomeAsync<TSome>(this Task<OptionsAssertions<TSome>> test, string because = "", params object[] becauseArgs)
			=> (await test).BeSome(because, becauseArgs);

		public static async Task<AndConstraint<OptionsAssertions<TSome>>> BeSomeAsync<TSome>(this Task<OptionsAssertions<TSome>> test, TSome value, string because = "", params object[] becauseArgs)
			=> (await test).BeSome(value, because, becauseArgs);

		public static async Task<AndConstraint<OptionsAssertions<TSome>>> BeNoneAsync<TSome>(this Task<OptionsAssertions<TSome>> test, string because = "", params object[] becauseArgs)
			=> (await test).BeNone(because, becauseArgs);
	}
}
