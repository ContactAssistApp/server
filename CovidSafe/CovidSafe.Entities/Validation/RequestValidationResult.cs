using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using ProtoBuf;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Object validation summary
    /// </summary>
    [JsonObject]
    [ProtoContract]
    public class RequestValidationResult
    {
        /// <summary>
        /// Tells if the result passed (true if yes, false if no)
        /// </summary>
        [JsonIgnore]
        [ProtoIgnore]
        public bool Passed
        {
            get
            {
                return this.Failures.Count == 0;
            }
        }

        /// <summary>
        /// Collection of <see cref="RequestValidationFailure"/> objects
        /// </summary>
        [JsonProperty("validationFailures")]
        [ProtoMember(1)]
        public List<RequestValidationFailure> Failures { get; set; } = new List<RequestValidationFailure>();

        /// <summary>
        /// Add <see cref="RequestValidationResult"/> failures to this object
        /// </summary>
        public void Combine(RequestValidationResult other)
        {
            this.Failures.AddRange(other.Failures);
        }

        /// <summary>
        /// Report a new <see cref="RequestValidationFailure"/>
        /// </summary>
        /// <param name="issue"><see cref="RequestValidationIssue"/> classification</param>
        /// <param name="property">Failing property name</param>
        /// <param name="message">Failure text</param>
        public void Fail(RequestValidationIssue issue, string property, string message)
        {
            this.Failures.Add(new RequestValidationFailure
            {
                Issue = issue,
                Message = message
            });
        }

        /// <summary>
        /// Report a new <see cref="RequestValidationFailure"/>
        /// </summary>
        /// <param name="issue"><see cref="RequestValidationIssue"/> classification</param>
        /// <param name="property">Failing property name</param>
        /// <param name="message">Failure text</param>
        /// <param name="args">Failure text arguments (akin to <see cref="String.Format(string, object[])"/></param>
        public void Fail(RequestValidationIssue issue, string property, string message, params string[] args)
        {
            // Use overload
            this.Fail(issue, property, String.Format(message, args));
        }
    }
}