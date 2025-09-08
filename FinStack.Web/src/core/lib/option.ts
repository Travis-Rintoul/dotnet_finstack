const TAG: unique symbol = Symbol('Option.tag');
const VAL: unique symbol = Symbol('Option.value');

export type Option<T> = Some<T> | None;

export interface Some<T> {
    isSome(this: Option<T>): this is Some<T>;
    isNone(this: Option<T>): this is None;
    map<B>(f: (a: T) => B): Option<B>;
    flatMap<B>(f: (a: T) => Option<B>): Option<B>;
    unwrap(): T;
    unwrapOr(fallback: T): T;
    match<R>(b: { some: (v: T) => R; none: () => R }): R;
}

export interface None {
    isSome(this: Option<unknown>): this is Some<never>;
    isNone(this: Option<unknown>): this is None;
    map<B>(_f: (a: never) => B): Option<B>;
    flatMap<B>(_f: (a: never) => Option<B>): Option<B>;
    unwrap(): never;
    unwrapOr<B>(fallback: B): B;
    match<R>(b: { some: (v: never) => R; none: () => R }): R;
}

interface SomeInternal<T> extends Some<T> {
    [TAG]: 'some';
    [VAL]: T;
}
interface NoneInternal extends None {
    [TAG]: 'none';
}

const SomeProto: Omit<SomeInternal<any>, typeof TAG | typeof VAL> = {
    isSome(this: Option<any>): this is Some<any> { return true; },
    isNone(this: Option<any>): this is None { return false; },

    map<B>(this: SomeInternal<any>, f: (a: any) => B): Option<B> {
        return some(f(this[VAL]));
    },
    flatMap<B>(this: SomeInternal<any>, f: (a: any) => Option<B>): Option<B> {
        return f(this[VAL]);
    },
    unwrap(this: SomeInternal<any>) { return this[VAL]; },
    unwrapOr(this: SomeInternal<any>, _fallback: any) { return this[VAL]; },

    match<R>(this: SomeInternal<any>, b: { some: (v: any) => R; none: () => R }): R {
        return b.some(this[VAL]);
    },
};

const NoneProto: Omit<NoneInternal, typeof TAG> = {
    isSome(this: Option<unknown>): this is Some<never> { return false; },
    isNone(this: Option<unknown>): this is None { return true; },

    map() { return none; },
    flatMap() { return none; },
    unwrap(): never { throw new Error('Tried to unwrap None'); },
    unwrapOr<B>(fallback: B): B { return fallback; },

    match<R>(b: { some: (v: never) => R; none: () => R }): R {
        return b.none();
    },
};

export function some<T>(value: T): Option<T> {
    const o = Object.create(SomeProto) as SomeInternal<T>;
    Object.defineProperties(o, {
        [TAG]: { value: 'some' },
        [VAL]: { value, writable: false },
    });
    return o;
}

export const none: Option<never> = (() => {
    const o = Object.create(NoneProto) as NoneInternal;
    Object.defineProperties(o, {
        [TAG]: { value: 'none' },
    });
    return o;
})();

export function match<T, R>(
    opt: Option<T>,
    branches: { some: (v: T) => R; none: () => R }
): R {
    return opt.match(branches);
}