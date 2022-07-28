﻿namespace BookStore.Application.Sales.ShoppingCarts.Commands.RemoveBook;

using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Exceptions;
using Common.Models;
using Domain.Sales.Exceptions;
using Domain.Sales.Repositories;
using MediatR;

public class ShoppingCartRemoveBookCommand : IRequest<Result>
{
    public int BookId { get; init; }

    public class ShoppingCartRemoveBookCommandHandler : IRequestHandler<ShoppingCartRemoveBookCommand, Result>
    {
        private readonly ICurrentUser currentUser;
        private readonly IBookDomainRepository bookRepository;
        private readonly ICustomerDomainRepository customerRepository;
        private readonly IShoppingCartDomainRepository shoppingCartRepository;

        public ShoppingCartRemoveBookCommandHandler(
            ICurrentUser currentUser,
            IBookDomainRepository bookRepository,
            ICustomerDomainRepository customerRepository,
            IShoppingCartDomainRepository shoppingCartRepository)
        {
            this.currentUser = currentUser;
            this.bookRepository = bookRepository;
            this.customerRepository = customerRepository;
            this.shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Result> Handle(
            ShoppingCartRemoveBookCommand request,
            CancellationToken cancellationToken)
        {
            var customerId = await this.customerRepository.GetCustomerId(
                this.currentUser.UserId,
                cancellationToken);

            var shoppingCart = await this.shoppingCartRepository.FindByCustomer(
                customerId,
                cancellationToken);

            if (shoppingCart is null)
            {
                throw new InvalidShoppingCartException(
                    $"Customer '{customerId}' does not have an existing shopping cart.");
            }

            var book = await this.bookRepository.Find(
                request.BookId,
                cancellationToken);

            if (book is null)
            {
                throw new NotFoundException(
                    nameof(book),
                    request.BookId);
            }

            shoppingCart.RemoveBook(book);

            return Result.Success;
        }
    }
}