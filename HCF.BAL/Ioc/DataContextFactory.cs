using System;

namespace HCF.BAL.Ioc
{    

    public static class UnityContextFactory<T>
    {
        /// <summary>
        /// The factory used to create an instance
        /// </summary>
        static Func<T> factory;

        /// <summary>
        /// Initializes the specified creation factory.
        /// </summary>
        /// <param name="creationFactory">The creation factory.</param>
        public static void SetFactory(Func<T> creationFactory)
        {
            factory = creationFactory;
        }

        /// <summary>
        /// Creates a new IMyDataContext instance.
        /// </summary>
        /// <returns>Returns an instance of an IMyDataContext </returns>
        public static T CreateContext()
        {
            if (factory == null) throw new InvalidOperationException("You can not create a context without first building the factory.");

            return factory();
        }
    }
}
