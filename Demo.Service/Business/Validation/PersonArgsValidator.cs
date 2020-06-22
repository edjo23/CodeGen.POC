#nullable enable

using Beef.Validation;
using Demo.Service.Common.Entities;

namespace Demo.Service.Business.Validation
{
    /// <summary>
    /// Represents a <see cref="PersonArgs"/> validator.
    /// </summary>
    public class PersonArgsValidator : Validator<PersonArgs, PersonArgsValidator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonArgsValidator"/>.
        /// </summary>
        public PersonArgsValidator()
        {
            Property(x => x.FirstName).Common(CommonValidator.Create<string?>(v => v.String(50)));
            Property(x => x.LastName).Common(CommonValidator.Create<string?>(v => v.String(50)));
            //Property(x => x.Genders).AreValid();
        }
    }
}

#nullable restore