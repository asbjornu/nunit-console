// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace NUnit.Framework
{
    /// <summary>
    /// Summary description for SetUICultureAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
    public class SetUICultureAttribute : PropertyAttribute, ICommandDecorator
    {
        /// <summary>
        /// Construct given the name of a culture
        /// </summary>
        /// <param name="culture"></param>
        public SetUICultureAttribute(string culture) : base("SetUICulture", culture) { }

        #region ICommandDecorator Members

        CommandStage ICommandDecorator.Stage
        {
            get { return CommandStage.SetContext; }
        }

        int ICommandDecorator.Priority
        {
            get { return 0; }
        }

        TestCommand ICommandDecorator.Decorate(TestCommand command)
        {
            return new SetUICultureCommand(command);
        }

        #endregion

        #region Nested Command Class

        private class SetUICultureCommand : DelegatingTestCommand
        {
            public SetUICultureCommand(TestCommand command)
                : base(command)
            {
            }

            public override TestResult Execute(object testObject, ITestListener listener)
            {
                string setUICulture = (string)Test.Properties.Get(PropertyNames.SetUICulture);
                if (setUICulture != null)
                    TestExecutionContext.CurrentContext.CurrentUICulture =
                        new System.Globalization.CultureInfo(setUICulture);

                return innerCommand.Execute(testObject, listener);
            }
        }

        #endregion
    }
}
