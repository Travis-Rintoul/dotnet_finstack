import { ok, err, Result, isResult } from './result';

describe('Result', () => {

    it('Ok should be Ok', () => {
        const result = ok('foo');
        expect(result.isOk()).toBe(true);
        expect(result.isErr()).toBe(false);
    });

    it('Err should be Err', () => {
        const result = err('foo');
        expect(result.isOk()).toBe(false);
        expect(result.isErr()).toBe(true);
    });

    it('map over Ok', () => {
        const result = ok(2).map(n => n + 1);
        expect(result.unwrap()).toBe(3);
    });

    it('map over Err', () => {
        const result = err(1).map(n => n + 1);
        expect(() => result.unwrap()).toThrowError(Error, 'Tried to unwrap Err');
    });

    it('Some should unwrap', () => {
        const option = ok('foo');
        expect(option.unwrap()).toBe('foo');
    });

    it('Err should error unwrap', () => {
        const option = err(null);
        expect(() => option.unwrap()).toThrowError(Error, 'Tried to unwrap Err');
    });

    it('map on Ok transforms', () => {
        const result = ok(2).map(n => n * 3).unwrap();
        expect(result).toBe(6);
    });

    it('map on Err is no-op', () => {
        const result = err<number, string>('boom').map(n => n + 1);
        expect(() => result.unwrap()).toThrow();
    });

    it('isResult should return correct value', () => {
        var result: Result<string, Error> = ok("test");
        var obj = { foo: 'bar' } as any;
        expect(isResult<string, Error>(result)).toBe(true);
        expect(isResult<number, Error>(obj)).toBe(false);
    });

    it('match branches', () => {
        const a = ok('x').match(v => `ok:${v}`, e => `err:${e}`);
        const b = err('bad').match(v => `ok:${v}`, e => `err:${e}`);
        expect(a).toBe('ok:x');
        expect(b).toBe('err:bad');
    });
});
