﻿#pragma checksum "..\..\..\..\..\..\..\..\..\..\Libs\wpftoolkit-master\ExtendedWPFToolkitSolution\Src\Xceed.Wpf.Toolkit.LiveExplorer\Samples\MaterialControls\Views\MaterialListBoxView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "26F4C99442F4A78A574EAFC1B6FA9F03494DC4C19A2380CD075FFDFAFBD8C07E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit.LiveExplorer;


namespace Xceed.Wpf.Toolkit.LiveExplorer.Samples.MaterialControls.Views {
    
    
    /// <summary>
    /// MaterialListBoxView
    /// </summary>
    public partial class MaterialListBoxView : Xceed.Wpf.Toolkit.LiveExplorer.MaterialDemoView, System.Windows.Markup.IComponentConnector {
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ReBirth;component/libs/wpftoolkit-master/extendedwpftoolkitsolution/src/xceed.wp" +
                    "f.toolkit.liveexplorer/samples/materialcontrols/views/materiallistboxview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\..\..\..\..\Libs\wpftoolkit-master\ExtendedWPFToolkitSolution\Src\Xceed.Wpf.Toolkit.LiveExplorer\Samples\MaterialControls\Views\MaterialListBoxView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 34 "..\..\..\..\..\..\..\..\..\..\Libs\wpftoolkit-master\ExtendedWPFToolkitSolution\Src\Xceed.Wpf.Toolkit.LiveExplorer\Samples\MaterialControls\Views\MaterialListBoxView.xaml"
            ((System.Windows.Documents.Hyperlink)(target)).RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(this.Hyperlink_RequestNavigate);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

