﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class GeradorPassagemPendenteParkResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal GeradorPassagemPendenteParkResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GeradorPassagensPendentesParkBatch.CommandQuery.Resources.GeradorPassagemPendente" +
                            "ParkResource", typeof(GeradorPassagemPendenteParkResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Processando passagens da concessionaria {0}.
        /// </summary>
        public static string Concessionaria {
            get {
                return ResourceManager.GetString("Concessionaria", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Enviando total de {0} passagens pendentes para a fila {1}..
        /// </summary>
        public static string EnviandoPassagem {
            get {
                return ResourceManager.GetString("EnviandoPassagem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Erro: {0}.
        /// </summary>
        public static string Error {
            get {
                return ResourceManager.GetString("Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Final do processo.
        /// </summary>
        public static string FinalProcesso {
            get {
                return ResourceManager.GetString("FinalProcesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Inicio do processo.
        /// </summary>
        public static string InicioProcesso {
            get {
                return ResourceManager.GetString("InicioProcesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Nenhuma passagem pendente da concessionaria {0}.
        /// </summary>
        public static string SemPassagensPendentes {
            get {
                return ResourceManager.GetString("SemPassagensPendentes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GeradorPassagensPendentesParkBatch - Fila {0} recebeu com sucesso o total de {1} passagens pendentes.
        /// </summary>
        public static string SucessoEnvio {
            get {
                return ResourceManager.GetString("SucessoEnvio", resourceCulture);
            }
        }
    }
}
