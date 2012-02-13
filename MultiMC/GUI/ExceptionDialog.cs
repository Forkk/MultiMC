// 
//  Copyright 2012  Andrew Okin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;

namespace MultiMC
{
	public partial class ExceptionDialog : Gtk.Dialog
	{
		public ExceptionDialog(Exception error)
		{
			this.Build();
			
			this.Error = error;
		}
		
		public Exception Error
		{
			get { return _error; }
			protected set
			{
				_error = value;
				textviewMessage.Buffer.Text = 
					GetMessageForException(value) + "\n" +
						"Please note that choosing to continue using the application " +
						"could cause undesired behavior including additional crashes.\n" +
						"To continue using the application, click cancel.\n" +
						"To abort, click quit.\n";
				infoTextview.Buffer.Text = _error.ToString();
			}
		}

		Exception _error;
		
		public static string GetMessageForException(Exception e)
		{
			if (e is NullReferenceException)
				return "A null reference exception occurred. This is usually the developer's fault. " +
					"Please report this crash.";
			else if (e is AccessViolationException)
				return "An access violation occurred. This usually indicates a threading issue " +
					"or some other serious programming issue. " +
					"Please report this bug to the developer.";
			else if (e is NotImplementedException)
				return "A not implemented exception occurred. This means the feature you were " +
					"trying to use is not implemented. Please report this bug to the developer.";
			else
				return "An unknown exception occurred. The exception type was " + 
					e.GetType().ToString();
		}

		protected void OnResponse(object o, Gtk.ResponseArgs args)
		{
			Destroy();
		}
	}
}

