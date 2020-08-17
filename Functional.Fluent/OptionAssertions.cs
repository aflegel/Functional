using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Functional.Fluent
{
	public class OptionsAssertions<T> : ReferenceTypeAssertions<Option<T>, OptionsAssertions<T>>
	{
		private T Exception => throw new Exception("Unexpected Result");

		private Exception InvokeSubjectWithInterception(Func<T, T> some, Func<T> none)
		{
			try
			{
				Subject.Match(some, none);
			}
			catch (Exception exc)
			{
				return exc;
			}
			return null;
		}

		public OptionsAssertions(Option<T> instance) => Subject = instance;

		protected override string Identifier => "option";

		public AndConstraint<OptionsAssertions<T>> BeSome(string because = "", params object[] becauseArgs)
		{
			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.Given(() => InvokeSubjectWithInterception(s => s, () => Exception))
				.ForCondition(s => s == null)
				.FailWith("Expected {context:option} to be Some but found None.");
			return new AndConstraint<OptionsAssertions<T>>(this);
		}

		public AndConstraint<OptionsAssertions<T>> BeSome(T value, string because = "", params object[] becauseArgs)
		{
			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.Given(() => InvokeSubjectWithInterception(s => s, () => Exception))
				.ForCondition(s => s == null)
				.FailWith("Expected {context:option} to be {0} but found None.", _ => value);

			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.Given(() => Subject.Match(s => s, () => Exception))
				.ForCondition(s => s.Equals(value))
				.FailWith("Expected {context:option} to be {0} but found {1}.", _ => value, s => s);

			return new AndConstraint<OptionsAssertions<T>>(this);
		}

		public AndConstraint<OptionsAssertions<T>> BeNone(string because = "", params object[] becauseArgs)
		{
			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.Given(() => InvokeSubjectWithInterception(s => s, () => Exception))
				.ForCondition(s => s != null)
				.FailWith("Expected {context:option} to be None but found Some.");
			return new AndConstraint<OptionsAssertions<T>>(this);
		}
	}
}
