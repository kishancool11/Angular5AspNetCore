import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable()
export class AlertifyService {

constructor() { }

    confirm (message: string, okCallback: () => any) {
        alertify.confirm(message, function(e){
            if (e) {
                okCallback();
            } else {
            }
        });
    }

    success(message: string) {
        alertify.success(message);
    }

    error(message: string) {
        alertify.error(message);
    }

    waringing(message: string) {
        alertify.waringing(message);
    }

    message(message: string) {
        alertify.message(message);
    }
}
