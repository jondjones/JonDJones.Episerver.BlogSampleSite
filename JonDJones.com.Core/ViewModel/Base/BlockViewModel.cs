using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Interfaces;
using JonDJones.Com.Core.Routing;
using System;

namespace JonDJones.Com.Core.ViewModel.Base
{
    public class BlockViewModel<T> : IBlockViewModel<T>
        where T : BlockData
    {
        internal IEpiServerDependencies _epiServerDependencies;

        public BlockViewModel(T currentBlock, IEpiServerDependencies epiServerDependencies)
        {
            _epiServerDependencies = epiServerDependencies;

            if (currentBlock == null)
                throw new ArgumentNullException("currentBlock");

            CurrentBlock = currentBlock;
        }

        public T CurrentBlock { get; private set; }

        protected IContentRepository ContentRepository
        {
            get
            {
                return _epiServerDependencies.ContentRepository;
            }
        }


        protected ILinkResolver LinkResolver
        {
            get
            {
                return _epiServerDependencies.LinkResolver;
            }
        }
    }
}