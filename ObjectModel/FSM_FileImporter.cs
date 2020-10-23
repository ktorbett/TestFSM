using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using KJT.Architecture.FiniteStateMachine;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;

namespace TestFSM {
    class FSM_FileImporter {

        class INNER_TRANS {
            internal string transPart;
            internal string fromStateName;
            internal STT_State fromState;
            internal string toStateName;
            internal STT_State toState;
            internal string eventName;
            internal string transAnno;
            internal string guardAnno;

            internal void parseTransitionString( FSM_STT theSTT, string s) {
                string[] aStr = s.Split(":");
                switch(aStr.Length) {

                    case 1:
                        // no ':' so only transParts present
                        this.transPart = aStr[0].Trim().Replace("\"", "");
                        break;

                    case 2:
                        // we have transition and event name with a non ':' separator
                        string[] bStr = aStr[1].Split(' ', '/', '\\', ']');
                        if(bStr.Length > 1) { // we have a split

                            if(Regex.IsMatch(aStr[1], @"\s*\[.*\].*")) {// check if aStr[1] contains []
                                            // in which case we have an event guard
                                this.eventName = bStr[1].Trim().Replace("\"", ""); // may be empty ?
                                this.guardAnno = bStr[0].Trim().Replace("\"", "") + "]";
                            } else {
                                this.eventName = bStr[0].Trim().Replace("\"", ""); // may be empty ?
                                this.transAnno = bStr[1].Trim().Replace("\"", "");
                            }

                            this.transPart = aStr[0].Trim().Replace("\"", "");
                            // ok normal

                        } else { // just an eventname ...

                            this.transAnno = "";// may be empty ?
                            this.eventName = bStr[0].Trim().Replace("\"", "");
                            this.transPart = aStr[0].Trim().Replace("\"", "");
                        }
                        break;

                    case 3:
                        // colon is separator for event and action?
                        this.transAnno = aStr[2].Trim().Replace("\"", "");
                        this.eventName = aStr[1].Trim().Replace("\"", "");
                        this.transPart = aStr[0].Trim().Replace("\"", "");

                        break;

                    default:
                        // ???
                        Debug.WriteLine("FSM_FileImporter.AddTransitionsFromArray() " +
                            "failure parsing a transition string ' " + s + " '");
                        break;
                }
                // then in transParts, the two events are divided by a two
                // character string that could be -> >> => == << <- <= 
                // is this a job for regex ?
                this.ExtractFromAndToState( theSTT);

                // at this point we should be ready to add transitions
                // but for the initial state, set the initialstate of the stt instead
                if( this.fromStateName == "initial") {
                    theSTT.setInitialState(this.toState);
                } else if ( this.fromState != null && this.toState != null && this.eventName != null ){
                    this.fromState.addTransition(this.eventName, this.toState);
                    // add the annotations ...
                    STT_Transition trans = null;
                    
                    fromState.getAllowedTransitions().TryGetValue(this.eventName, out trans);
                    if ( trans != null) {
                        trans.setGuardAnnotation( this.guardAnno);
                        trans.setTransitionAnnotation( this.transAnno);
                    }
                }

            }

            private void ExtractFromAndToState( FSM_STT theSTT) {
                // start with this.transPart.  parse it...
                // using ([A-Za-z ]+)((?:=>)|(?:->)|(?:==)|(?:>>)|(?:<-)|(?:<=)|(?:<<))([A-Za-z ]+)
                // or (.+)((?:=>)|(?:->)|(?:==)|(?:>>)|(?:<-)|(?:<=)|(?:<<))(.+)
                string pattern = @"(.+)((?:=>)|(?:->)|(?:==)|(?:>>)|(?:<-)|(?:<=)|(?:<<))(.+)";

                Match match = Regex.Match(this.transPart, pattern);
                if ( match.Success ) {

                    // check the direction of the transition
                    string dir = match.Groups[2].Value;
                    if( dir == "->" || dir == "=>" || dir == ">>" | dir == "==" ) {
                        this.fromStateName = match.Groups[1].Value;
                        this.toStateName = match.Groups[3].Value;
                    } else {
                        this.fromStateName = match.Groups[3].Value;
                        this.toStateName = match.Groups[1].Value;
                    }

                    this.fromState = findOrCreateStateNameInSTT( theSTT, fromStateName);
                    this.toState = findOrCreateStateNameInSTT(theSTT, toStateName);
                    
                                        
                } else {
                    Debug.WriteLine("INNER_TRANS.ExtractFromAndTostate() error parsing transPart string " + this.transPart);
                }
            }
        }


        static FSM_STT theSTT = null;

        public static STT_State findOrCreateStateNameInSTT( FSM_STT theSTT, string stateName) {
            STT_State retVal = null;

            foreach ( STT_State state in theSTT.getStatesList()) {
                if ( state.getStateName() == stateName) {
                    retVal = state;
                }
            }
            if ( retVal == null) {
                // create it - 'cos it's mentioned in a transition but not in the state section
                retVal = theSTT.addState(stateName);
            }

            return retVal;
        }

        public static void SMCatImport( string fileName ) { // probably need an array of options as well
            // 
            string[] inputLines = File.ReadAllLines(fileName);

            StringCollection strColHead = new StringCollection();
            StringCollection strColBody = new StringCollection();

            SplitHeaderAndBody(inputLines, strColHead, strColBody);

            theSTT = CreateSTT(strColHead);

            ExtractAndAddStatesToSTT( FSM_FileImporter.theSTT, strColBody);
                                 
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

        private static void ExtractAndAddStatesToSTT( FSM_STT theSTT, StringCollection bodyText) {

            string noUnwantedSpaces = ""; 
            StringCollection tmp = new StringCollection();
            // what happens in here ....  replace spaces but not inside ""
            foreach ( string s in bodyText) {
                tmp.Add( DealWithSpaces(s));
            }
            // then make the string collection into a single string
            // so we can split it
            string[] strArray = new string[tmp.Count];
            tmp.CopyTo( strArray,0);
            noUnwantedSpaces = String.Concat(strArray);
            
            string[] statesAndTransitions = noUnwantedSpaces.Split(";");

            // at this stage statesAndTransitions[0] is the list of states and their comment strings like onEntry exit etc. 
            // as well as their decorations like the []^ to mark them as fork join or decision.
            // the rest ( lines 1-n ) hold transitions. 
            // So we shall further split the states up by parsing between commas. ( use Split () )

            string[] states = statesAndTransitions[0].Split(",");
            
            AddStatesFromArray( theSTT, states);

            string[] transitions = new string[statesAndTransitions.Length -1 ];

            transitions = statesAndTransitions[1..statesAndTransitions.Length];

            // now make a list of transitions
            // them match them with the States we have
            // and any left over, create new States

            AddTransitionsFromArray( theSTT, transitions);
        }

        private static void AddTransitionsFromArray(FSM_STT theSTT , string[] transitions) {
            //

            List<INNER_TRANS> iTransList = new List<INNER_TRANS>(transitions.Length);
                
            int count = 0;
            foreach( string s in transitions) {
                // pick out the parts to the left and right 
                // the rightmost part after the ':' is the event name
                iTransList.Add(new INNER_TRANS());
                iTransList[count].parseTransitionString(theSTT, s);
                count++;
            }
        }

      
        private static void AddStatesFromArray( FSM_STT theSTT, string[] states) {

            foreach ( string s in states ) {

                string[] strArray = s.Split(":");
                string annotations = "";
                string stateName = strArray[0].Trim().Replace("\"", "");
                if(strArray.Length > 1) {
                    annotations = strArray[1].Trim();
                }
                if(stateName != "initial" && stateName != "final") {

                    // TODO - when we implement join fork and decision
                    // here is where we check the first char of the stateName
                    // and create the sub-class instance instead SST_State_Join
                    // STT_State_Fork or STT_State_Decision.   
                    //

                    STT_State state = theSTT.addState(stateName);

                    //Now if there is annotation set it up.

                    if( annotations !="" ) {

                        // it might have a split on \r\n then use onEntry()/ entry() onEntry entry
                        // and so on....   maybe this is a regex job. ?
                        // these ought to pull out all the options for onEntry onExit etc.
                        // (?:on)?[Ee]ntry(?:\(\))?[\/\\: ](.*)
                        //(?:on)?[Ee]xit(?:\(\))?[\/\\: ](.*)

                        Match match = null;
                        match = Regex.Match(annotations, @"(?:on)?[Ee]ntry(?:\(\))?[\/\\: ](.*)");
                        string onEntryAnn = match.Groups[1].Value.Trim();
                        if( onEntryAnn != "" && onEntryAnn != null) {
                            state.setOnEntryAnnotation( onEntryAnn);
                        }
                        match = Regex.Match(annotations, @"(?:on)?[Ee]xit(?:\(\))?[\/\\: ](.*)");
                        string onExitAnn = match.Groups[1].Value.Trim();
                        if(onExitAnn != "" && onExitAnn != null) {
                            state.setOnExitAnnotation( onExitAnn);
                        }

                    }

                }
            }
        }

        private static string DealWithSpaces(string s) {

            // removes spaces outside quotes, replaces with underscores inside
            
            StringBuilder newValue = new StringBuilder("");
            string[] sets = s.Split('\"');
            for(int i = 0; i < sets.Length; i++) {
                if(i % 2 == 0)
                    // even ones are outside quotes
                    newValue.Append(sets[i].Replace(" ", ""));
                else
                    // and the odd ones are in quotes
                    newValue.Append("\"" + sets[i].Replace(" ", "_") + "\"");
                ;
            }

            // final " ?
            return newValue.ToString();  // was thinking of adding trim() on the end, 
            //but it deletes \r\n we WANT that are inside a state comment between : and ;
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
