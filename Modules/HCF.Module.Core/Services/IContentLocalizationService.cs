using HCF.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Services
{
    public interface IContentLocalizationService
    {
        string GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue) where TEntity : EntityBase;

        string GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, string cultureId) where TEntity : EntityBase;

        string GetLocalizedProperty(string entityType, long entityId, string propertyName, string propertyValue);

        string GetLocalizedProperty(string entityType, long entityId, string propertyName, string propertyValue, string cultureId);

        Func<long, string, string, string> GetLocalizationFunction<TEntity>();

        Func<long, string, string, string> GetLocalizationFunction(string entityType);
    }
}
