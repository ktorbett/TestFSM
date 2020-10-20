using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using KJT.Architecture.FiniteStateMachine;

namespace TestFSM {
    class FSM_FileImporter {

        public static void SMCatImport( string fileName ) { // probably need an array of options as well
            // 
            string text = File.ReadAllText(fileName);

            //  "[^"]*"(*SKIP)(*F)|\s+

            //Regex r = new Regex("\"[^\"]* \"(*SKIP)(*F)|\\s+");

            // strip whitespace ...
            string noWSText = text.Replace(" ", "");

            // if it leaves carriage returns it should be ok ..
            // so convert text into string array.
            string[] lines = text.Split("\n");

            // then parse line by line using patterns ?   OR are we going to parse as a single string 

        }
    }
}
