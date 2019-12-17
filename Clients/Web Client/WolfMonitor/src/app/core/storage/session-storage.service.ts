import { Injectable, Inject } from '@angular/core';
import { SessionStorageKeys } from './session-storage.enum';

@Injectable()
export class SessionStorageService {
    constructor( @Inject(Window) private windowRef: Window) {

    }

    public setValue(key: SessionStorageKeys, value: any) : void {
        this.windowRef.sessionStorage.setItem(key.toString(), JSON.stringify(value));
    }
    public getValue(key: SessionStorageKeys) : any {
        return JSON.parse(this.windowRef.sessionStorage.getItem(key.toString()));
    }

    public deleteValue(key: SessionStorageKeys) : void {
        delete this.windowRef.sessionStorage[key.toString()];
    }
}
