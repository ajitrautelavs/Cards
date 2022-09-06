import { Component, OnInit } from '@angular/core';
import { Card } from './models/card.model';
import { CardsService } from './service/cards.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Cards.UI';
  cards: Card[] = [];
  card: Card = {
    id: '',
    cardHolderName: '',
    cardNumber: '',
    expiryMonth: '',
    expiryYear: '',
    cvc:''
  };

  constructor(private cardsService: CardsService) {

  }
  ngOnInit(): void {
    this.getAllCards();
  }

  getAllCards() {
    this.cardsService.getAllCards()
      .subscribe(
        Response => {
          this.cards = Response;
        }
    );
  }

  onSubmit() {
    if (this.card.id === '') {
      this.cardsService.addCard(this.card)
        .subscribe(
          Response => {
            this.getAllCards();

            this.card = {
              id: '',
              cardHolderName: '',
              cardNumber: '',
              expiryMonth: '',
              expiryYear: '',
              cvc: ''
            };
          }
        )
    }
    else {
      this.cardsService.updateCard(this.card)
      .subscribe(
        Response => {
          this.getAllCards();

          this.card = {
            id: '',
            cardHolderName: '',
            cardNumber: '',
            expiryMonth: '',
            expiryYear: '',
            cvc: ''
          };
        }
      )
    }
    
  }

  deleteCard(id: string) {
    this.cardsService.deleteCard(id)
      .subscribe(
        Response => {
          this.getAllCards();

        }
    )
  }

  populateForm(card: Card) {
    this.card = card;
  }
}
