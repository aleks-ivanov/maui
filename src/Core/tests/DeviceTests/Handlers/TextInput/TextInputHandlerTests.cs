﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public abstract partial class TextInputHandlerTests<THandler, TStub> : HandlerTestBase<THandler, TStub>
		where THandler : IViewHandler
		where TStub : StubBase, ITextInputStub, new()
	{
		[Theory(DisplayName = "TextChanged Events Fire Correctly")]
		// null/empty
		[InlineData(null, null, false)]
		[InlineData(null, "", false)]
		[InlineData("", null, false)]
		[InlineData("", "", false)]
		// whitespace
		[InlineData(null, " ", true)]
		[InlineData("", " ", true)]
		[InlineData(" ", null, true)]
		[InlineData(" ", "", true)]
		[InlineData(" ", " ", false)]
		// text
		[InlineData(null, "Hello", true)]
		[InlineData("", "Hello", true)]
		[InlineData(" ", "Hello", true)]
		[InlineData("Hello", null, true)]
		[InlineData("Hello", "", true)]
		[InlineData("Hello", " ", true)]
		[InlineData("Hello", "Goodbye", true)]
		public async Task TextChangedEventsFireCorrectly(string initialText, string newText, bool eventExpected)
		{
			var textInput = Activator.CreateInstance<TStub>();
			textInput.Text = initialText;

			var eventFiredCount = 0;
			textInput.TextChanged += (sender, e) =>
			{
				eventFiredCount++;

				Assert.Equal(initialText, e.OldValue);
				Assert.Equal(newText ?? string.Empty, e.NewValue);
			};

			await SetValueAsync(textInput, newText, SetNativeText);

			if (eventExpected)
				Assert.Equal(1, eventFiredCount);
			else
				Assert.Equal(0, eventFiredCount);
		}

		protected abstract void SetNativeText(THandler entryHandler, string text);
	}
}