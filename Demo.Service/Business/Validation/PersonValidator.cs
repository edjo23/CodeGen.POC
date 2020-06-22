#nullable enable

using Beef.Validation;
using Demo.Service.Common.Entities;

namespace Demo.Service.Business.Validation
{
    /// <summary>
    /// Represents a <see cref="Person"/> validator.
    /// </summary>
    public class PersonValidator : Validator<Person, PersonValidator>
    {
        //private static readonly Validator<Address> _addressValidator = Validator.Create<Address>()
        //    .HasProperty(x => x.Street, p => p.Mandatory().Common(CommonValidators.Text))
        //    .HasProperty(x => x.City, p => p.Mandatory().Common(CommonValidators.Text));

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonValidator"/>.
        /// </summary>
        public PersonValidator()
        {
            Property(x => x.FirstName).Mandatory().Common(CommonValidator.Create<string?>(v => v.String(50)));
            Property(x => x.LastName).Mandatory().Common(CommonValidator.Create<string?>(v => v.String(50)));
            //Property(x => x.Gender).Mandatory().IsValid();
            //Property(x => x.EyeColor).IsValid();
            //Property(x => x.Birthday).Mandatory().CompareValue(CompareOperator.LessThanEqual, DateTime.Now, "Today");
            //Property(x => x.Address).Entity(_addressValidator);
        }
    }
}

#nullable restore