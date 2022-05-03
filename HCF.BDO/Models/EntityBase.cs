using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO.Models
{
    public abstract class EntityBase : EntityBaseWithTypedId<long>
    {
    }

    public abstract class EntityBaseWithTypedId<TId> : ValidatableObject, IEntityWithTypedId<TId>
    {
        public virtual TId Id { get; protected set; }
    }

    public abstract class ValidatableObject
    {
        public virtual bool IsValid()
        {
            return Validate().Count == 0;
        }

        public virtual IList<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this, null, null), validationResults, true);
            return validationResults;
        }
    }
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
