import  { Injectable, Inject } from '@angular/core';
import  { LocalStorageKeys } from './local-storage.enum';

@Injectable()
export class LocalStorageService {

    constructor(@Inject(Window) private windowRef:Window) {

    }

    public getValue(key: LocalStorageKeys): any {
        return this.windowRef.localStorage.getItem(key.toString());

    }
    
    public setValue(key: LocalStorageKeys, value: any) : void {
        this.windowRef.localStorage.setItem(key.toString(), value);
    }

    public deleteValue(key: LocalStorageKeys) : void {
        delete this.windowRef.localStorage[key.toString()];
    }
}