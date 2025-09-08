import { catchError, map, of, OperatorFunction } from "rxjs";

const TAG: unique symbol = Symbol('Result.tag');
const VAL: unique symbol = Symbol('Result.value');
const ERR: unique symbol = Symbol('Result.error');

export type Result<T, E> = Ok<T, E> | Err<T, E>;

export interface Ok<T, E = never> {
    isOk(this: Result<T, E>): this is Ok<T, E>;
    isErr(this: Result<T, E>): this is Err<T, E>;
    map<U>(f: (t: T) => U): Result<U, E>;
    mapErr<F>(_f: (e: E) => F): Result<T, F>;
    flatMap<U>(f: (t: T) => Result<U, E>): Result<U, E>;
    unwrap(): T;
    unwrapOr(fallback: T): T;
    match<R>(ok: (v: T) => R, err: (e: E) => R): R;
}

export interface Err<T = never, E = unknown> {
    isOk(this: Result<T, E>): this is Ok<T, E>;
    isErr(this: Result<T, E>): this is Err<T, E>;
    map<U>(_f: (t: T) => U): Result<U, E>;
    mapErr<F>(f: (e: E) => F): Result<T, F>;
    flatMap<U>(_f: (t: T) => Result<U, E>): Result<U, E>;
    unwrap(): never;
    unwrapOr(fallback: T): T;
    match<R>(ok: (v: T) => R, err: (e: E) => R): R;
}

interface OkInternal<T, E> extends Ok<T, E> {
    [TAG]: 'ok';
    [VAL]: T;
}
interface ErrInternal<T, E> extends Err<T, E> {
    [TAG]: 'err';
    [ERR]: E;
}

const OkProto: Omit<OkInternal<any, any>, typeof TAG | typeof VAL> = {
    isOk(this: Result<any, any>): this is Ok<any, any> { return true; },
    isErr(this: Result<any, any>): this is Err<any, any> { return false; },

    map<U>(this: OkInternal<any, any>, f: (t: any) => U): Result<U, any> {
        return ok(f(this[VAL]));
    },
    mapErr<F>(this: OkInternal<any, any>, _f: (e: any) => F): Result<any, F> {
        return ok<any, F>(this[VAL]);
    },
    flatMap<U>(this: OkInternal<any, any>, f: (t: any) => Result<U, any>): Result<U, any> {
        return f(this[VAL]);
    },
    unwrap(this: OkInternal<any, any>) { return this[VAL]; },
    unwrapOr(this: OkInternal<any, any>, _fallback: any) { return this[VAL]; },

    match<R>(this: OkInternal<any, any>, okFn: (v: any) => R, _errFn: (e: any) => R): R {

        
        console.log('qqq', this[VAL]);
        return okFn(this[VAL]);
    },
};

const ErrProto: Omit<ErrInternal<any, any>, typeof TAG | typeof ERR> = {
    isOk(this: Result<any, any>): this is Ok<any, any> { return false; },
    isErr(this: Result<any, any>): this is Err<any, any> { return true; },

    map<U>(this: ErrInternal<any, any>, _f: (t: any) => U): Result<U, any> {
        return this as any;
    },
    mapErr<F>(this: ErrInternal<any, any>, f: (e: any) => F): Result<any, F> {
        return err(f(this[ERR]));
    },
    flatMap<U>(this: ErrInternal<any, any>, _f: (t: any) => Result<U, any>): Result<U, any> {
        return this as any;
    },
    unwrap(): never { throw new Error('Tried to unwrap Err'); },
    unwrapOr<T>(this: ErrInternal<T, any>, fallback: T): T { return fallback; },

    match<R>(this: ErrInternal<any, any>, _okFn: (v: any) => R, errFn: (e: any) => R): R {
        return errFn(this[ERR]);
    },
};

export function ok<T, E = never>(value: T): Result<T, E> {
    const o = Object.create(OkProto) as OkInternal<T, E>;
    Object.defineProperties(o, {
        [TAG]: { value: 'ok' },
        [VAL]: { value, writable: false },
    });
    return o;
}

export function err<T = never, E = unknown>(error: E): Result<T, E> {
    const o = Object.create(ErrProto) as ErrInternal<T, E>;
    Object.defineProperties(o, {
        [TAG]: { value: 'err' },
        [ERR]: { value: error, writable: false },
    });
    return o;
}

export function match<T, E, R>(
    res: Result<T, E>,
    okFn: (v: T) => R,
    errFn: (e: E) => R
): R {
    return res.match(okFn, errFn);
}

export function asResult<T, E = unknown>(
    mapError?: (e: unknown) => E
): OperatorFunction<T, Result<T, E>> {
    return (source$) =>
        source$.pipe(
            map((v) => ok<T, E>(v)),
            catchError((e) => of(err<T, E>(mapError ? mapError(e) : (e as E))))
        );
}

export function isResult<T, E>(val: unknown): val is Result<T, E> {
    return !!val && typeof val === 'object' &&
        (((val as any)[TAG] === 'ok') || ((val as any)[TAG] === 'err'));
}
