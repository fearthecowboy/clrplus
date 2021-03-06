﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2010-2013 Garrett Serack and CoApp Contributors. 
//     Contributors can be discovered using the 'git log' command.
//     All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

namespace Scratch {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using ClrPlus.Powershell.Core;
    using ClrPlus.Scripting.Languages.PropertySheetV3;

#if FALSE

    class MSBuildFile {
        
    }

    class Pivot {
        string Key;
        Dictionary<string, string[]> Choices;

    }

    class Package {
        Dictionary<string, Pivot> Pivots;
        MSBuildFile Operations;
    }

    internal class MSBuildPropertyModel : PropertyModel {
        public MSBuildPropertyModel(MSBuildFile buildFile) {
            
        }

        public override bool CanAddProperty(string p) {
            throw new NotImplementedException();
        }

        public override bool HasProperty(string p) {
            throw new NotImplementedException();
        }

        public override PropertyModel GetPropertyReference(Selector selector) {
            var result = base.GetPropertyReference(selector);
            if (result == null) {
                
            }
            return result;
        }

        public override object Value {get; set;}

        public virtual bool SetProperty(string p, object o) {
            throw new NotImplementedException();
        }

        public virtual bool ClearProperty(string p) {
            throw new NotImplementedException();
        }

        public override bool IsList {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsArray { get {
            return false;
        } }

        public override bool Add(object value) {
            throw new NotImplementedException();
        }

        public override bool IsDictionary {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool Set(object key, object value) {
            throw new NotImplementedException();
        }
    };


    internal class Program {
        private static void Main(string[] args) {
            var p = new Package();

            PropertySheet.Load("test.props", new DynamicPropertyModel(p) {
                Strict = false,
                Handlers = new Dictionary<Type, ModelFactory> {
                    {typeof (MSBuildFile), obj => new MSBuildPropertyModel(obj as MSBuildFile) }
                }
            });
        }
    }
#endif

    class MSBuildProject {
        
    }

    class Autopackage {
        public MSBuildProject Project;


    }



    internal class Program {

        public object SomeLookup(string param) {
            return null;
        }
        private static void Main(string[] args) {
            new Program().Start(args);
        }
        private void Start(string[] args) {
            try {
                var tests = new[] {
                    @"tests\pass\Alias_decl.txt", @"tests\pass\Coll_ops.txt", @"tests\pass\Dict_ops.txt", @"tests\pass\test.txt"
                };

                foreach (var t in tests) {
                    IModel model = new DataModel();
                    // model[new Selector { Name = "Project" }] = new ProjectModel();

                    Console.WriteLine("\r\n\r\n == TEST: {0} ==", t);
                    model.ParseFile(t);

                    var autopkg = new Autopackage();

                    var virtualPackageMap = model.MapTo(autopkg, new {
                        Project = (Route)((context, selector) => new ViewObject(autopkg.Project, new {
                            @case = (Route)((project,caseSelector) => new ViewObject(() => {
                                // bla bla bla ... code that looks up the right object
                                var resultObject = SomeLookup(caseSelector.Parameter); //[ x86, release]
                                return resultObject;
                            }, new {
                              // routes for the children of this object. 
                                @defines = (Route)((caseObj,propSelector) => new ViewProperty<object>(() => {
                                    // getter
                                    var define = propSelector.Parameter;
                                    var ctx = caseObj;
                                    dynamic resultObject = SomeLookup("a function that returns the the thing that is a ");
                                    return resultObject.GetDefine();
                                }, (value) => {
                                    // setter
                                    var define = propSelector.Parameter;
                                    var ctx = caseObj;
                                    dynamic resultObject = SomeLookup("a function that returns the the thing that is a ");
                                    resultObject.SetDefine(value);
                                }))
                            }))
                        })),

                        @default = (Route)((context,selector) => {
                            if(selector.Name == "Candy") {
                                return new ViewObject(SomeLookup("someCandyObject"));
                            }
                            return null; // let it figure it out
                        } )
                    });

                    var routes = new {
                        Project = (Route)((context, selector) => new ViewObject(autopkg.Project, new {
                            @case = (Route)((project, caseSelector) => new ViewObject(() => {
                                // bla bla bla ... code that looks up the right object
                                var resultObject = SomeLookup(caseSelector.Parameter); //[ x86, release]
                                return resultObject;
                            }, new {
                                // routes for the children of this object. 
                                @defines = (Route)((caseObj, propSelector) => new ViewProperty<string> (() => {
                                    // getter
                                    var define = propSelector.Parameter;
                                    var ctx = caseObj;
                                    dynamic resultObject = SomeLookup("a function that returns the the thing that is a ");
                                    return resultObject.GetDefine();
                                }, (value) => {
                                    // setter
                                    var define = propSelector.Parameter;
                                    var ctx = caseObj;
                                    dynamic resultObject = SomeLookup("a function that returns the the thing that is a ");
                                    resultObject.SetDefine(value);
                                }))
                            }))
                        })),

                        @default = (Route)((context, selector) => {
                            if (selector.Name == "Candy") {
                                return new ViewObject(SomeLookup("someCandyObject"));
                            }
                            return null; // let it figure it out
                        })
                    };

                    // var mappedObject = model.MapTo(autopkg).AddRoutes(routes, null);
                    

                    var someprop = virtualPackageMap.SomeObject.SomeProperty;

                    IEnumerable<Selector> objnames = virtualPackageMap.GetChildSelectors();
                    IEnumerable<string> objnames2 = virtualPackageMap.GetChildNames();
                    var propnames= virtualPackageMap.GetPropertyNames();
                    var propnames2 = virtualPackageMap.GetPropertySelectors();

                    virtualPackageMap.Project.CopyToBackingObject(true); 


                    foreach (var i in objnames.Where(each => each.Name == "case")) {
                        
                    }
                    
                    var thatcase = virtualPackageMap.Project.@case["x86", "release"];
                    var defines = thatcase.Defines;

                }
            } catch (Exception e) {
                Console.WriteLine("{0} =>\r\n\r\nat {1}", e.Message, e.StackTrace.Replace("at ClrPlus.Scripting.Languages.PropertySheetV3.PropertySheetParser", "PropertySheetParser"));
            }
            return;
        }
    }

    [Cmdlet(AllVerbs.Add, "Nothing")]
    public class AddNothingCmdlet : PSCmdlet {
        protected override void ProcessRecord() {
            using (var ps = Runspace.DefaultRunspace.Dynamic()) {
                var results = ps.GetItemss("c:\\");
                foreach (var item in results) {
                    Console.WriteLine(item);
                }
            }
        }
    }
}

/*
                   * 
      public View(Func<object> backingObjectAccessor, params Route[] routes) {
          _backingObjectAccessor = backingObjectAccessor;
      }

      public View(object backingObject, params Route[] routes) {
          _backingObjectAccessor = () => backingObject;
      }

                  var map = model.MapTo(autopkg,
                      pkgSelector => {
                          if (pkgSelector.Name == "Project") {
                              return new View(autopkg.Project,
                                  sel => {
                                      if (sel.Name == "case") {
                                          return new View(() => {
                                              // bla bla bla ... code that looks up the right object
                                              var resultObject = SomeLookup(sel.Parameter);
                                              return resultObject; 
                                          });
                                      }
                                      return null;
                                  });
                          }
                          return null;
                      });


                  var map2 = model.MapTo(autopkg, new {
                      @Project = (Route)(pkgSelector => {
                          return new View(autopkg.Project, new {
                              @case = (Route)(sel => {
                                  return new View(() => {
                                      // bla bla bla ... code that looks up the right object
                                      var resultObject = SomeLookup(sel.Parameter);
                                      return resultObject; 
                                  });
                              })
                          });
                      })
                  });
                  */