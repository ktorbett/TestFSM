﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using KJT.Architecture.FiniteStateMachine;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TestFSM {
    class FSM_FileImporter {


        static FSM_STT theSTT = null;

        public static void SMCatImport( string fileName ) { // probably need an array of options as well
            // 
            string[] inputLines = File.ReadAllLines(fileName);

            //string inputString = File.ReadAllText(fileName);

            StringCollection strColHead = new StringCollection();
            StringCollection strColBody = new StringCollection();
            StringCollection strColStates = null;
            StringCollection strColTransitions = null;

            SplitHeaderAndBody(inputLines, strColHead, strColBody);

            theSTT = CreateSTT(strColHead);

            strColStates = ExtractStates(strColBody);


            //  Now i think we need create single string from strcolstates ( strColBody ) -
            //  actually it is still in an internmediate states.   we need to split it into ; separated items
            // and probably a single string BUT we want to preserve the crlf ( \r\n) IF they are in between a : and a ,
            // now that might be a job for a regex ....

        }

        private static FSM_STT CreateSTT(StringCollection strColHead) {
            Match match = null;


            string STT_refClassName = "";
            string STT_nameSpace = "";
            string STT_vctString = "";

            // Process the Header to get values for the STT creation:
            foreach(string s in strColHead) {
                // see if it contains a value for STT refClassName
                if(STT_refClassName == "") {
                    match = Regex.Match(s, @"^#FSM:(.*)$");
                    if(match.Success) {
                        STT_refClassName = match.Groups[1].Value.Trim();
                    }
                }

                // see if it contains a value for STT_nameSpace
                if(STT_nameSpace == "") {
                    match = Regex.Match(s, @"^#nameSpace:(.*)$");
                    if(match.Success) {
                        STT_nameSpace = match.Groups[1].Value.Trim();
                    }
                }

                // see if it contains a value for STT_vctString
                if(STT_vctString == "") {
                    match = Regex.Match(s, @"^#vctString:(.*)$");
                    if(match.Success) {
                        STT_vctString = match.Groups[1].Value.Trim();
                    }
                }

                

            }

            // check we have enough
            if(STT_nameSpace == "" || STT_refClassName == "" | STT_vctString == "") {
                Debug.WriteLine("FSM_FileInporter.createSTT() - nameSpace, refClasName or vctString null");
                return null;
            }
            return new FSM_STT(STT_refClassName, STT_nameSpace, STT_vctString);
        }

        private static StringCollection ExtractStates(StringCollection bodyText) {


            string pattern = "";  // this has to match a 
            StringCollection retVal = new StringCollection();
            // what happens in here ....
            foreach ( string s in bodyText) {
                retVal.Add( ReplaceSpacesOutsideStrings(s));
            }


            return retVal;
        }

        private static string ReplaceSpacesOutsideStrings(string s) {
            
            StringBuilder newValue = new StringBuilder("");
            string[] sets = s.Split('\"');
            for(int i = 0; i < sets.Length; i++) {
                if(i % 2 == 0)
                    // even ones are outside quotes
                    newValue.Append(sets[i].Replace(" ", ""));
                else
                    // and the odd ones are in quotes
                    newValue.Append("\"" + sets[i] + "\"");
            }

            // final %
            return newValue.ToString();
        }

        private static void SplitHeaderAndBody(string[] strArray, StringCollection strColHead, StringCollection strColBody ) {

            string pattern = "(^#.*$?)";
            foreach( string s in strArray ) {
                // does it start with a # and end with  \r\n ?
                if ( Regex.IsMatch( s, pattern)) {
                    strColHead.Add(s + "\r\n");
                } else {
                    strColBody.Add(s + "\r\n");
                }
            }
        }

        
         
    }
}
