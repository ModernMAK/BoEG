using System;
using UnityEngine;

namespace MobaGame.UI
{
    public static class ExceptionX
	{
        private static string CreateMissingComponentExceptionMessage(string gameObjectName, string componentName) => $"{gameObjectName} is missing component {componentName}!";
        public static MissingComponentException CreateMissingComponentException(string gameObjectName, string componentName, Exception innerException) => new MissingComponentException(CreateMissingComponentExceptionMessage(gameObjectName, componentName), innerException);
        public static MissingComponentException CreateMissingComponentException(string gameObjectName, string componentName) => new MissingComponentException(CreateMissingComponentExceptionMessage(gameObjectName, componentName));
    }
}