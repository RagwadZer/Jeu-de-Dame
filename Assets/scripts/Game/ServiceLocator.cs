using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ServiceLocator
{
    //pour �viter d'avoir des singletons partout, ainsi qu'un couplage trop fort entre les scripts
    //on utilise un service Locator pour g�rer de mani�re centralis� les r�f�rence.

    /// <summary>
    /// contients un acc�s vers les scripts utilis�s fr�quemment
    /// </summary>
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    /// <summary>
    /// enregistre service de classe T dans mon dictionnaire services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (!services.ContainsKey(type))
        {
            services[type] = service;
            //Debug.Log($"Service {type.Name} enregistr�.");
        }
    }

    /// <summary>
    /// renvoie le service (script) de classe T enregistr� dans le dictionnaire services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T Get<T>()
    {
        if(services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new Exception($"aucun script de type {typeof(T)} enregistr�");
    }

}
