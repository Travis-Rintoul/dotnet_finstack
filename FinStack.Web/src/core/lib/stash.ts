import { BehaviorSubject, Subject, Observable, EMPTY, tap, catchError, finalize, map } from 'rxjs';
import { isResult, ok, Result } from './result';

export class Stash<T, E = unknown> {
    private readonly _value = new BehaviorSubject<T[]>([]);
    private readonly _loading = new BehaviorSubject<boolean>(false);
    private readonly _saving = new BehaviorSubject<boolean>(false);
    private readonly _loaded = new Subject<void>();
    private readonly _saved = new Subject<void>();
    private readonly _error = new Subject<E>();

    readonly value$ = this._value.asObservable();
    readonly loading$ = this._loading.asObservable();
    readonly saving$ = this._saving.asObservable();
    readonly loaded$ = this._loaded.asObservable();
    readonly saved$ = this._saved.asObservable();
    readonly error$ = this._error.asObservable();

    load(stream$: Observable<T[]>): void;
    load(stream$: Observable<Result<T[], E>>): void;
    load(stream$: Observable<T[] | Result<T[], E>>): void {
        this._loading.next(true);
        stream$
        .pipe(
            map(value => (isResult<T[], E>(value) ? value : ok(value))),
            tap(result => {
                result.match(
                    data => {
                        console.log('ok', data);
                        this._value.next([...data] as T[]);
                        this._loaded.next();
                    },
                    error => {
                        console.log('error', error);
                        this._error.next(error);
                    }
                )
            }),
            catchError(err => {
                this._error.next(err);
                return EMPTY;
            }),
            finalize(() => this._loading.next(false))
        )
        .subscribe();
    }
}
