using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAutomatization.Core.JS {
    public struct JavaScriptEvent {
        public readonly string Name;
        public JavaScriptEvent(string name) : this() { Name = name; }

        public static implicit operator string(JavaScriptEvent x) {
            return x.Name;
        }

        public override string ToString() { return Name; }

        public readonly static JavaScriptEvent KeyUp = new JavaScriptEvent("keyup");
        public readonly static JavaScriptEvent Click = new JavaScriptEvent("click");

    }
}
