#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LabelManager.cs" company="FAIRYTALE-DISTILLERY.COM">
//     Copyright © 2015 FAIRYTALE-DISTILLERY.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY FAIRYTALE-DISTILLERY.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL FAIRYTALE-DISTILLERY.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>
//   Class LabelManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;

    using Artemis.Utils;

    #endregion Using statements

    /// <summary>Each entity can have many or none labels attached to it. This keeps track of all labels.</summary>
    public sealed class LabelManager
    {
        /// <summary>The entities by label.</summary>
        private readonly Dictionary<string, HashSet<Entity>> entitiesByLabel;

        /// <summary>Initializes a new instance of the <see cref="LabelManager" /> class.</summary>
        internal LabelManager()
        {
            this.entitiesByLabel = new Dictionary<string, HashSet<Entity>>();
        }

        /// <summary>Gets the entitys that have this label assigned to.</summary>
        /// <param name="label">The label.</param>
        /// <returns>The specified entity.</returns>
        public Bag<Entity> GetEntities(string label)
        {
            Debug.Assert(!string.IsNullOrEmpty(label), "Label must not be null or empty.");

            Bag<Entity> entitiesBag = new Bag<Entity>();

            HashSet<Entity> entities = null;
            if (entitiesByLabel.TryGetValue(label, out entities))
            {
                if (entities != null)
                {
                    foreach(var entity in entities)
                    {
                        entitiesBag.Add(entity);
                    }
                }
            }

            return entitiesBag; 
        }

        /// <summary>Gets the labels of entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The labels of the specified entity.</returns>
        public Bag<string> GetLabelsOfEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            Bag<string> labels = new Bag<string>();
            foreach (var pair in entitiesByLabel)
            {
                if (pair.Value != null && pair.Value.Contains(entity))
                {
                    labels.Add(pair.Key);
                }
            }

            return labels;
        }

        /// <summary>
        /// Queries if the given label is assigned to the entity
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="entity">The entity</param>
        /// <returns></returns>
        public bool HasLabel(string label, Entity entity)
        {
            HashSet<Entity> entities = null;
            if (entitiesByLabel.TryGetValue(label, out entities))
            {
                if (entities.Contains(entity)) return true;
            }

            return false;
        }

        /// <summary>Adds the the specified label to the entity.</summary>
        /// <param name="label">The label.</param>
        /// <param name="entity">The entity.</param>
        public void Add(string label, Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(!string.IsNullOrEmpty(label), "Label must not be null or empty.");

            HashSet<Entity> entities = null;
            if (entitiesByLabel.TryGetValue(label, out entities))
            {
                entities.Add(entity);
            }
            else
            {
                entities = new HashSet<Entity>();
                entities.Add(entity);
                entitiesByLabel[label] = entities;
            }
        }

        /// <summary>Removes the specified label from the label.</summary>
        /// <param name="label">The label.</param>
        /// <param name="entity">The entity.</param>
        public void Remove(string label, Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(!string.IsNullOrEmpty(label), "Label must not be null or empty.");

            HashSet<Entity> entities = null;
            if (entitiesByLabel.TryGetValue(label, out entities))
            {
                entities.Remove(entity);
            }
        }

        /// <summary>Unregisters the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        internal void Unregister(Entity entity)
        {
            foreach (var pair in entitiesByLabel)
            {
                if (pair.Value != null && pair.Value.Contains(entity))
                {
                    pair.Value.Remove(entity);
                }
            }
        }
    }
}