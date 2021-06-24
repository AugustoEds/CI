﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace InventarioCxParadaService.YepLogWebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ActionToolUiSoap", Namespace="http://tempuri.org/")]
    public partial class ActionToolUi : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendEmailOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailAttachmentOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailSeenOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailYepLogOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ActionToolUi() {
            this.Url = global::InventarioCxParadaService.Properties.Settings.Default.InventarioCxParadaService_YepLogWebReference_ActionToolUi;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SendEmailCompletedEventHandler SendEmailCompleted;
        
        /// <remarks/>
        public event SendEmailAttachmentCompletedEventHandler SendEmailAttachmentCompleted;
        
        /// <remarks/>
        public event SendEmailSeenCompletedEventHandler SendEmailSeenCompleted;
        
        /// <remarks/>
        public event SendEmailYepLogCompletedEventHandler SendEmailYepLogCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEmail", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ResponseDto SendEmail(string subject, string content, string to, string descriptionAccount) {
            object[] results = this.Invoke("SendEmail", new object[] {
                        subject,
                        content,
                        to,
                        descriptionAccount});
            return ((ResponseDto)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailAsync(string subject, string content, string to, string descriptionAccount) {
            this.SendEmailAsync(subject, content, to, descriptionAccount, null);
        }
        
        /// <remarks/>
        public void SendEmailAsync(string subject, string content, string to, string descriptionAccount, object userState) {
            if ((this.SendEmailOperationCompleted == null)) {
                this.SendEmailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailOperationCompleted);
            }
            this.InvokeAsync("SendEmail", new object[] {
                        subject,
                        content,
                        to,
                        descriptionAccount}, this.SendEmailOperationCompleted, userState);
        }
        
        private void OnSendEmailOperationCompleted(object arg) {
            if ((this.SendEmailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailCompleted(this, new SendEmailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEmailAttachment", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ResponseDto SendEmailAttachment(string subject, string content, string to, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] attachment, FileExtensionDto fileExtension, string descriptionAccount) {
            object[] results = this.Invoke("SendEmailAttachment", new object[] {
                        subject,
                        content,
                        to,
                        attachment,
                        fileExtension,
                        descriptionAccount});
            return ((ResponseDto)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailAttachmentAsync(string subject, string content, string to, byte[] attachment, FileExtensionDto fileExtension, string descriptionAccount) {
            this.SendEmailAttachmentAsync(subject, content, to, attachment, fileExtension, descriptionAccount, null);
        }
        
        /// <remarks/>
        public void SendEmailAttachmentAsync(string subject, string content, string to, byte[] attachment, FileExtensionDto fileExtension, string descriptionAccount, object userState) {
            if ((this.SendEmailAttachmentOperationCompleted == null)) {
                this.SendEmailAttachmentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailAttachmentOperationCompleted);
            }
            this.InvokeAsync("SendEmailAttachment", new object[] {
                        subject,
                        content,
                        to,
                        attachment,
                        fileExtension,
                        descriptionAccount}, this.SendEmailAttachmentOperationCompleted, userState);
        }
        
        private void OnSendEmailAttachmentOperationCompleted(object arg) {
            if ((this.SendEmailAttachmentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailAttachmentCompleted(this, new SendEmailAttachmentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEmailSeen", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ResponseDto SendEmailSeen(EmailSenderDto emailSender) {
            object[] results = this.Invoke("SendEmailSeen", new object[] {
                        emailSender});
            return ((ResponseDto)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailSeenAsync(EmailSenderDto emailSender) {
            this.SendEmailSeenAsync(emailSender, null);
        }
        
        /// <remarks/>
        public void SendEmailSeenAsync(EmailSenderDto emailSender, object userState) {
            if ((this.SendEmailSeenOperationCompleted == null)) {
                this.SendEmailSeenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailSeenOperationCompleted);
            }
            this.InvokeAsync("SendEmailSeen", new object[] {
                        emailSender}, this.SendEmailSeenOperationCompleted, userState);
        }
        
        private void OnSendEmailSeenOperationCompleted(object arg) {
            if ((this.SendEmailSeenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailSeenCompleted(this, new SendEmailSeenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEmailYepLog", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ResponseDto SendEmailYepLog(EmailSenderDto emailSender) {
            object[] results = this.Invoke("SendEmailYepLog", new object[] {
                        emailSender});
            return ((ResponseDto)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailYepLogAsync(EmailSenderDto emailSender) {
            this.SendEmailYepLogAsync(emailSender, null);
        }
        
        /// <remarks/>
        public void SendEmailYepLogAsync(EmailSenderDto emailSender, object userState) {
            if ((this.SendEmailYepLogOperationCompleted == null)) {
                this.SendEmailYepLogOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailYepLogOperationCompleted);
            }
            this.InvokeAsync("SendEmailYepLog", new object[] {
                        emailSender}, this.SendEmailYepLogOperationCompleted, userState);
        }
        
        private void OnSendEmailYepLogOperationCompleted(object arg) {
            if ((this.SendEmailYepLogCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailYepLogCompleted(this, new SendEmailYepLogCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ResponseDto {
        
        private ResponseTypeDto typeField;
        
        private string messageField;
        
        private object objReturnField;
        
        /// <remarks/>
        public ResponseTypeDto Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public object ObjReturn {
            get {
                return this.objReturnField;
            }
            set {
                this.objReturnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum ResponseTypeDto {
        
        /// <remarks/>
        Success,
        
        /// <remarks/>
        BusinessRule,
        
        /// <remarks/>
        Exception,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class AttachmentDto {
        
        private byte[] attachmentField;
        
        private FileExtensionDto fileExtensionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Attachment {
            get {
                return this.attachmentField;
            }
            set {
                this.attachmentField = value;
            }
        }
        
        /// <remarks/>
        public FileExtensionDto FileExtension {
            get {
                return this.fileExtensionField;
            }
            set {
                this.fileExtensionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum FileExtensionDto {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        Xls,
        
        /// <remarks/>
        Txt,
        
        /// <remarks/>
        Pdf,
        
        /// <remarks/>
        Zip,
        
        /// <remarks/>
        Rar,
        
        /// <remarks/>
        Doc,
        
        /// <remarks/>
        Csv,
        
        /// <remarks/>
        Xlsx,
        
        /// <remarks/>
        Docx,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class EmailSenderDto {
        
        private string subjectField;
        
        private string contentField;
        
        private string[] lsToField;
        
        private string[] lsCarbonCopyField;
        
        private string[] lsBlindCarbonCopyField;
        
        private string descriptionAccountField;
        
        private AttachmentDto[] lsAttachmentField;
        
        /// <remarks/>
        public string Subject {
            get {
                return this.subjectField;
            }
            set {
                this.subjectField = value;
            }
        }
        
        /// <remarks/>
        public string Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        public string[] LsTo {
            get {
                return this.lsToField;
            }
            set {
                this.lsToField = value;
            }
        }
        
        /// <remarks/>
        public string[] LsCarbonCopy {
            get {
                return this.lsCarbonCopyField;
            }
            set {
                this.lsCarbonCopyField = value;
            }
        }
        
        /// <remarks/>
        public string[] LsBlindCarbonCopy {
            get {
                return this.lsBlindCarbonCopyField;
            }
            set {
                this.lsBlindCarbonCopyField = value;
            }
        }
        
        /// <remarks/>
        public string DescriptionAccount {
            get {
                return this.descriptionAccountField;
            }
            set {
                this.descriptionAccountField = value;
            }
        }
        
        /// <remarks/>
        public AttachmentDto[] LsAttachment {
            get {
                return this.lsAttachmentField;
            }
            set {
                this.lsAttachmentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    public delegate void SendEmailCompletedEventHandler(object sender, SendEmailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResponseDto Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResponseDto)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    public delegate void SendEmailAttachmentCompletedEventHandler(object sender, SendEmailAttachmentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailAttachmentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailAttachmentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResponseDto Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResponseDto)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    public delegate void SendEmailSeenCompletedEventHandler(object sender, SendEmailSeenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailSeenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailSeenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResponseDto Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResponseDto)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    public delegate void SendEmailYepLogCompletedEventHandler(object sender, SendEmailYepLogCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailYepLogCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailYepLogCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResponseDto Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResponseDto)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591