import { some, none } from './option';

describe('Option', () => {

    it('some should be Some', () => {
        const option = some('foo');
        expect(option.isSome()).toBe(true);
        expect(option.isNone()).toBe(false);
    });

    it('none should be None', () => {
        const option = none;
        expect(option.isSome()).toBe(false);
        expect(option.isNone()).toBe(true);
    });

    it('map over Some', () => {
        const option = some(2).map(n => n + 1);
        expect(option.unwrap()).toBe(3);
    });

    it('map over None', () => {
        const option = none.map(n => n + 1);
        expect(() => option.unwrap()).toThrowError(Error, 'Tried to unwrap None');
    });

    it('Some should unwrap', () => {
        const option = some('foo');
        expect(option.unwrap()).toBe('foo');
    });

    it('None should error unwrap', () => {
        const option = none;
        expect(() => option.unwrap()).toThrowError(Error, 'Tried to unwrap None');
    });

    it('match works', () => {
        const a = some('t').match({ some: v => v + '!', none: () => 'empty' });
        const b = none.match({ some: (v: string) => v, none: () => 'empty' });
        expect(a).toBe('t!');
        expect(b).toBe('empty');
    });
});
